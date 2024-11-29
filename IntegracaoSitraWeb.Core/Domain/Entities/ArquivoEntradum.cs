using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoSitraWeb.Core.Domain.Entities
{
    public class ArquivoEntradum
    {
        public int Id { get; set; }

        public string? Origem { get; set; }

        public string? NomeArquivo { get; set; }

        public byte? Status { get; set; }

        public DateTime? DataFtp { get; set; }

        public DateTime? DataInclusao { get; set; }

        public DateTime? DataInicioProcessamento { get; set; }

        public DateTime? DataFimProcessamento { get; set; }

        public int? QtdeOrdemIntegrados { get; set; }

        public virtual ICollection<Ordem> Ordems { get; set; } = new List<Ordem>();
    }
}
