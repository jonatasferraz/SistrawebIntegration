using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoSitraWeb.Core.Domain.Entities
{
    public partial class ArquivoSitraweb
    {
        public int Id { get; set; }

        public string? NomeArquivo { get; set; }

        public byte? Status { get; set; }

        public DateTime? DataInclusao { get; set; }

        public DateTime? DataEnvio { get; set; }
    }
}
