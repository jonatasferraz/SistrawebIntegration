using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoSitraWeb.Core.Domain.Entities
{
    public partial class Origem
    {
        public string Origem1 { get; set; } = null!;

        public string Tipo { get; set; } = null!;

        public string? Host { get; set; }

        public string? Usuario { get; set; }

        public string? Senha { get; set; }

        public string? DirNotfis { get; set; }

        public string? DirOcorren { get; set; }

        public string? DirConemb { get; set; }

        public string? DirDoccob { get; set; }
    }

}
