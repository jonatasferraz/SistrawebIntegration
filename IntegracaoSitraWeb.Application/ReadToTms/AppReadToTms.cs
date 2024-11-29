using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using IntegracaoSitraWeb.Core.Domain.Entities;
using IntegracaoSitraWeb.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IntegracaoSitraWeb.Application.ReadToTms
{
    public class AppReadToTms
    {
        private DiaslogNewContext _context;
        private Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.ArquivoEntradum> _RepArquivo;

        public AppReadToTms(DiaslogNewContext contexto) {

            _context = contexto;
           
        }
         

        public HashSet<string> GetNonExistingFiles(List<string> files, string origem)
        {

            if(files == null || files.Count == 0) return new HashSet<string>();

            
            var parametros = files
              .Select((item, index) => new SqlParameter($"@param{index}", item))
              .ToList();

            // Construir a parte do "IN" dinamicamente
            var inClause = string.Join(", ", parametros.Select(p => p.ParameterName));


            string query = $@"
                SELECT NomeArquivo
                FROM ARQUIVO_ENTRADA
                WHERE NomeArquivo IN ({inClause})
                AND origem = @origem
            ";

            parametros.Add(new SqlParameter("@origem", origem));


            List<ArquivoEntradum> result = _context.ArquivoEntrada.FromSqlRaw(query, parametros.ToArray()).ToList();
          
                        
            return (HashSet<string>)files.Where(item => !result.Any(r => r.NomeArquivo == item)).ToHashSet<string>();

        }
    }          
}
