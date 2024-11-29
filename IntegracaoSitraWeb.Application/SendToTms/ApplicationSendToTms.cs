using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoSitraWeb.Core.Domain.Entities;
using IntegracaoSitraWeb.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace IntegracaoSitraWeb.Application.SendToTms
{



    public class ApplicationSendToTms
    {
        private DiaslogNewContext _context;
        private Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.ArquivoEntradum> _RepArquivo;
        private Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.Ordem> _RepOrdem;
        internal Dictionary<int, string> templates = new()
        {
            // Linha 000
            { 000, "{0,-3}{1,-35}{2,-35}{3,-6}{4,-4}{5,-12}{6,-145}" },

            // Linha 310
            { 310, "{0,-3}{1,-14}{2,-223}" },

            // Linha 311
            { 311, "{0,-3}{1,-14}{2,-15}{3,-40}{4,-35}{5,-9}{6,-9}{7,-8}{8,-40}{9,-67}" },

            // Linha 312
            { 312, "{0,-3}{1,-40}{2,-14}{3,-15}{4,-40}{5,-20}{6,-35}{7,-9}{8,-9}{9,-9}{10,-4}{11,-35}{12,-1}{13,-5}" },
            //Linha 313 
            { 313, "{0,-3}{1,-15}{2,-7}{3,-1}{4,-1}{5,-1}{6,-1}{7,-3}{8,-8}{9,-8}{10,-15}{11,-15}{12,-7}{13,-15}{14,-7}{15,-5}{16,-1}{17,-1}{18,-15}{19,-15}{20,-7}{21,-1}{22,-15}{23,-15}{24,-15}{25,-15}{26,-1}{27,-12}{28,-12}{29,-1}{30,-44}" },
            // Linha 333
            { 333, "{0,-3}{1,-4}{2,-1}{3,-8}{4,-4}{5,-8}{6,-4}{7,-15}{8,-1}{9,-155}{10,-5}{11,-32}" },

            // Linha 314
            { 314, "{0,-3}{1,-7}{2,-15}{3,-30}{4,-7}{5,-15}{6,-30}{7,-7}{8,-15}{9,-30}{10,-7}{11,-15}{12,-30}{13,-29}" },

            // Linha 318
            { 318, "{0,-3}{1,-15}{2,-15}{3,-15}{4,-15}{5,-15}{6,-15}{7,-147}" }
        };
        internal int Pagina = 1;
        internal int PageSize = 200;

        public ApplicationSendToTms(DiaslogNewContext contexto)
        {
            _context = contexto;
            _RepOrdem = new GenericRepository<Ordem>(_context);
        }




        public Task Processar()
        {
            try
            {
                var Ordens = _RepOrdem.GetAllAsync().Result.ToList().Where(o => o != null &&
                o.STATUS_SITRAWEB == false && o.FaturamentoCnpj != null && o.RemetenteCnpj != null).ToList();


                var cnpjs = Ordens  // Filtra ordens onde StatusSitraweb = false
                              .Select(o => new { o.FaturamentoCnpj, o.RemetenteCnpj })  // Seleciona as propriedades desejadas
                              .Distinct()  // Garante que o resultado seja distinto (baseado nas duas propriedades)
                              .ToList();  // Converte o resultado em uma lista;

                var idsParaAtualizarStatus = new List<int>();


                for (int i = 0; i < cnpjs.Count; i++)
                {
                    bool temMaisRegistros = true;

                    while (temMaisRegistros)
                    {

                        var resultado = Paginar<Core.Domain.Entities.Ordem>(Ordens.Where(od => od.FaturamentoCnpj == cnpjs[i].FaturamentoCnpj && od.RemetenteCnpj == cnpjs[i].RemetenteCnpj).ToList().OrderBy(oo => oo.Id).ToList(), Pagina, PageSize).ToList();


                        if (resultado.Any())
                        {
                            var data = new List<(int LineCode, string[] Values)>
                            {
                                (000, new string[] { "000", $"{resultado.First().RemetenteRazao ?? string.Empty}", "SITRAWEB",
                                    $"{DateTime.Now.Date.ToString("ddMMyy")}", $"{DateTime.Now.Hour}{DateTime.Now.Minute}",
                                    $"NOT{DateTime.Now.ToString("ddMMhhmm")}S", string.Empty }),
                                (310, new string[] { "310", $"NOTFI{DateTime.Now.ToString("ddMMhhmm")}S", string.Empty }),
                                (311, new string[] { "311", $"{resultado.First().RemetenteCnpj?.ToString().PadLeft(14,'0')}",
                                    $"{resultado.First()?.RemetenteIe}", $"{resultado.First()?.RemetenteEnd ?? string.Empty}",
                                    $"{resultado.First().RemetenteCidade ?? string.Empty}", $"{resultado.First().RemetenteCep ?? 0}",
                                    $"{resultado.First().RemetenteUf ?? string.Empty}", $"{resultado.First().DataEmbarque?.ToString("ddMMyyyy") ?? string.Empty} ",
                                    "DIASLOG", string.Empty }),
                            };

                            foreach (var item in resultado)
                            {
                                data.AddRange(AdicionaOrdem(item, data));
                                idsParaAtualizarStatus.Add(item.Id);
                            }
                            Pagina += 1;

                            data.Add((318, new string[] { "318", $"{resultado.Sum(r => r.NfValor ?? 0) * 100:0#############0}",
                                $"{resultado.Sum(r => r.NfPeso ?? 0) * 100:0#############0}", "000000000000000",
                                $"{resultado.Sum(r => r.QtdeVolumes ?? 0) * 100:0#############0}",
                                $"{resultado.Sum(r => r.FaturamentoFrete ?? 0) * 100:0#############0}",
                                $"{resultado.Sum(r => r.FaturamentoGris ?? 0) * 100:0#############0}", string.Empty }));

                            var pathFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "ArquivosToSend");
                            string fileName = $"NOT{DateTime.Now.ToString("ddhhmm") 
                                + resultado.First().RemetenteCnpj?.ToString().PadLeft(14, '0') 
                                + resultado.First().FaturamentoCnpj?.ToString().PadLeft(14, '0')}S";
                            GenerateFile(data, templates, pathFile, fileName);
                        }
                        else
                        {
                            temMaisRegistros = false;
                            Pagina = 1;
                        }
                    }
                }
                //sendFileTxt();
                //if (idsParaAtualizarStatus.Any())
                //{
                //    string updateStatusSql = $@"
                //        UPDATE ORDEM 
                //        SET STATUS_SITRAWEB = 1
                //        WHERE ID IN ({string.Join(",", idsParaAtualizarStatus.Select(x => $"'{x}'"))})";

                //    //await connection.ExecuteAsync(updateStatusSql);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao buscar dados: {ex.Message}");
            }
            return Task.CompletedTask;
        }

        private void sendFileTxt()
        {
            throw new NotImplementedException();
        }

        internal void GenerateFile(List<(int LineCode, string[] Values)> data, Dictionary<int, string> templates, string filePath, string fileName)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            using (var writer = new StreamWriter(filePath + $"\\{fileName}"))
            {
                foreach (var entry in data)
                {
                    string line = BuildLine(entry.LineCode, entry.Values, templates);
                    writer.WriteLine(line);
                }
            }
        }

        internal string BuildLine(int lineCode, string[] values, Dictionary<int, string> templates)
        {
            if (!templates.ContainsKey(lineCode))
                throw new Exception($"Template para linha {lineCode} não encontrado.");

            var formattedValues = values.Select((v, i) => FormatField(v, GetFieldLength(i, lineCode))).ToArray();
            return string.Format(templates[lineCode], formattedValues);
        }

        internal string FormatField(string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                value = string.Empty;

            if (value.Length > length)
                return value.Substring(0, length);
            else
                return value.PadRight(length);
        }

        internal int GetFieldLength(int fieldIndex, int lineCode)
        {
            var fieldLengths = new Dictionary<int, int[]>
        {
            { 000, new int[] { 3, 35, 35, 6, 4, 12, 145 } }, // Linha 000
            { 310, new int[] { 3, 14, 223 } },               // Linha 310
            { 311, new int[] { 3, 14, 15, 40, 35, 9, 9, 8, 40, 67 } }, // Linha 311
            { 312, new int[] { 3, 40, 14, 15, 40, 20, 35, 9, 9, 9, 4, 35, 1, 6 } }, // Linha 312
            { 313, new int[] { 3, 15,7,1,1,1,1,3,8,8,15,15,7,15,7,5,1,1,15,15,7,1,15,15,15,15,1,12,12,1,44 } }, // Linha 313
            { 333, new int[] { 3, 4, 1, 8, 4, 8, 4, 15, 1, 155,5,32 } }, // Linha 333
            { 314, new int[] { 3, 7,15,30, 7, 15, 30, 7, 15, 30, 7, 15, 30,29 } }, // Linha 314
            { 318, new int[] { 3, 15, 15, 15, 15, 15, 15, 147 } } // Linha 318
        };
            return fieldLengths[lineCode][fieldIndex];
        }

        internal List<(int LineCode, string[] Values)>? PreencherLinhasMercadoria(Ordem ordem)
        {

            var resultado = ordem.Volumes;

            if (resultado.Count == 0) return null;
            var linhas = new List<(int LineCode, string[] Values)>();

            const int maxItensPorLinha = 4;

            for (int i = 0; i < resultado.Count; i += maxItensPorLinha)
            {
                i = 5;
                var volumesSubset = ordem.Volumes.Skip(i).Take(maxItensPorLinha)
                                        .Select(_ => "1".PadLeft(5, '0').PadRight(2, '0'))
                                        .Concat(Enumerable.Repeat(string.Empty, maxItensPorLinha))
                                        .Take(maxItensPorLinha)
                                        .ToArray();

                var mercadoriaSubset = ordem.Volumes.Skip(i).Take(maxItensPorLinha)
                                                    .Select(v => v.CodigoBarras ?? string.Empty)
                                                    .Concat(Enumerable.Repeat(string.Empty, maxItensPorLinha))
                                                    .Take(maxItensPorLinha)
                                                    .ToArray();

                var caixasSubset = ordem.Volumes.Skip(i).Take(maxItensPorLinha)
                                        .Select(_ => "CAIXAS")
                                        .Concat(Enumerable.Repeat(string.Empty, maxItensPorLinha))
                                        .Take(maxItensPorLinha)
                                        .ToArray();

                var linha314 = new string[]
                    {
                        "314",
                        volumesSubset[0], caixasSubset[0], mercadoriaSubset[0],
                        volumesSubset[1], caixasSubset[1], mercadoriaSubset[1],
                        volumesSubset[2], caixasSubset[2], mercadoriaSubset[2],
                        volumesSubset[3], caixasSubset[3], mercadoriaSubset[3],
                        string.Empty
                    };

                linhas.Add((314, linha314));
            }

            return linhas;
        }

        private List<(int LineCode, string[] Values)> AdicionaOrdem(Ordem ordem, List<(int LineCode, string[] Values)> data)
        {

            var newData = new List<(int LineCode, string[] Values)>
            {
                (312, new string[] {
                    "312",
                    $"{(ordem.DestinatarioNome ?? string.Empty).Substring(0, Math.Min((ordem.DestinatarioNome ?? string.Empty).Length, 40))} {ordem.DestinatarioCodigo}",
                    $"{(ordem.DestinatarioCpf?.ToString() ?? string.Empty).PadLeft(14, '0')}", $"{(ordem.DestinatarioIe ?? string.Empty)}", $"{(ordem.DestinatarioEnd ?? string.Empty)}",
                    $"{(ordem.DestinatarioBairro ?? string.Empty)}", $"{(ordem.DestinatarioCidade ?? string.Empty)}", $"{(ordem.DestinatarioCep?.ToString() ?? string.Empty)}",
                    $"{(ordem.DestinatarioCodMun?.ToString()  ?? string.Empty)}", $"{ordem.DestinatarioUf ?? string.Empty}",string.Empty,$"{(ordem.DestinatarioTelefone ?? string.Empty)}",
                    $"{(IsCnpj((ordem.DestinatarioCpf?.ToString() ?? string.Empty).PadLeft(14, '0')) ? '1' : '2')}", string.Empty }),
                (313, new string[] {
                    "313",$"{(ordem.NumeroRomaneio?.ToString() ?? string.Empty)}",$"{(ordem.SubrotaEntrega ?? string.Empty)}","1","1","2","C",
                    $"{(ordem.NfSerie?.ToString() ?? string.Empty)}",$"{(ordem.NfNumero?.ToString().PadLeft(8, '0'))}",$"{ordem.NfDataEmissao?.ToString("ddMMyyyy") ?? string.Empty}",
                    "DIVERSOS","CAIXAS",$"{(ordem.QtdeVolumes?.ToString().PadLeft(5, '0').PadRight(7, '0'))}",$"{(ordem.NfValor??0 * 100):0#############0}",
                    $"{(ordem.NfPeso??0 * 100):#00000#}","00000","S","N",$"{(ordem.FaturamentoGris??0 * 100):0#############0}",$"{(ordem.FaturamentoFrete??0 * 100):0#############0}",
                    string.Empty,"N","000000000000000",$"{(ordem.FaturamentoAdvalorem??0 * 100):0#############0}","000000000000000",$"{(ordem.FaturamentoFrete * 100):0#############0}",
                    "I","000000000000","000000000000","N",string.Empty}),
                (333, new string[] {
                    "333","0000","2",$"{ordem.DataPrazo?.ToString("ddMMyyyy") ?? string.Empty}","0000",$"{ordem.DataPrazo?.ToString("ddMMyyyy") ?? string.Empty}",
                    "2359",string.Empty,"N",string.Empty,"31",string.Empty})
                
            };

            var linhasMercadoria = PreencherLinhasMercadoria(ordem);
            if(linhasMercadoria!= null && linhasMercadoria.Count != 0) newData.AddRange(from linha in linhasMercadoria select linha);
            return newData;

        }

        public static List<T> Paginar<T>(List<T> lista, int pageNumber, int pageSize)
        {
            return lista
                .Skip((pageNumber - 1) * pageSize)  // Pula os itens das páginas anteriores
                .Take(pageSize)  // Pega os itens da página atual
                .ToList();  // Converte para lista
        }
        private static bool IsCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}
