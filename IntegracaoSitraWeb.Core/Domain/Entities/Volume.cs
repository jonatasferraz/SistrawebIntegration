using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoSitraWeb.Core.Domain.Entities
{
    public partial class Volume
    {
        public int Id { get; set; }

        public int? IdOrdem { get; set; }

        public int? NumeroVolume { get; set; }

        public string? CodigoBarras { get; set; }

        public string? Tipo { get; set; }

        public virtual Ordem? IdOrdemNavigation { get; set; }
    }
}
