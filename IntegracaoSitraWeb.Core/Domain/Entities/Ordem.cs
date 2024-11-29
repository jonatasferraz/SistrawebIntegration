using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IntegracaoSitraWeb.Core.Domain.Entities
{
    public partial class Ordem
    {
        public int Id { get; set; }

        public int? IdArquivoEntrada { get; set; }

        public int? NfNumero { get; set; }

        public byte? NfSerie { get; set; }

        public int? NfControle { get; set; }

        public DateOnly? NfDataEmissao { get; set; }

        public double? NfValor { get; set; }

        public double? NfPeso { get; set; }

        public string? PedidoTipo { get; set; }

        public DateOnly? PedidoData { get; set; }

        public long? PedidoNumero { get; set; }

        public long? RemetenteCnpj { get; set; }

        public string? RemetenteIe { get; set; }

        public string? RemetenteRazao { get; set; }

        public string? RemetenteEnd { get; set; }

        public string? RemetenteCidade { get; set; }

        public int? RemetenteCep { get; set; }

        public string? RemetenteUf { get; set; }

        public long? DestinatarioCodigo { get; set; }

        public string? DestinatarioNome { get; set; }

        public long? DestinatarioCpf { get; set; }

        public string? DestinatarioIe { get; set; }

        public string? DestinatarioEnd { get; set; }

        public string? DestinatarioCompl { get; set; }

        public string? DestinatarioBairro { get; set; }

        public string? DestinatarioCidade { get; set; }

        public int? DestinatarioCep { get; set; }

        public string? DestinatarioPontoReferencia { get; set; }

        public int? DestinatarioCodMun { get; set; }

        public string? DestinatarioUf { get; set; }

        public string? DestinatarioTelefone { get; set; }

        public string? DestinatarioEmail { get; set; }

        public string? RecebedorNome { get; set; }

        public long? RecebedorCpf { get; set; }

        public string? RecebedorEndereco { get; set; }

        public string? RecebedorCompl { get; set; }

        public string? RecebedorBairro { get; set; }

        public string? RecebedorCidade { get; set; }

        public int? RecebedorCep { get; set; }

        public int? RecebedorCodMun { get; set; }

        public string? RecebedorEstado { get; set; }

        public DateOnly? DataEmbarque { get; set; }

        public long? NumeroRomaneio { get; set; }

        public int? QtdeVolumes { get; set; }

        public double? ValorFrete { get; set; }

        public DateOnly? DataPrazo { get; set; }

        public string? Setor { get; set; }

        public int? CondicaoExpedicao { get; set; }

        public byte? GrauRisco { get; set; }

        public long? NumeroRemessa { get; set; }

        public string? NomePromotora { get; set; }

        public long? NumeroRomaneioRedespacho { get; set; }

        public int? CodigoTransportadora { get; set; }

        public string? SubrotaEntrega { get; set; }

        public int? SequenciaEntrega { get; set; }

        public byte? PeriodoEntrega { get; set; }

        public string? TipoTransporte { get; set; }

        public string? FaturamentoCnpj { get; set; }

        public double? FaturamentoFreteCalculado { get; set; }

        public double? FaturamentoFrete { get; set; }

        public double? FaturamentoGris { get; set; }

        public double? FaturamentoAdvalorem { get; set; }

        public virtual ArquivoEntradum? IdArquivoEntradaNavigation { get; set; }
        public virtual ICollection<Volume> Volumes { get; set; } = new List<Volume>();

        public Boolean? STATUS_SITRAWEB { get; set; }    
    }

}
