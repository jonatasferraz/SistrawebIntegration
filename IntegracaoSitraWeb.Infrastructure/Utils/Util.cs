using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoSitraWeb.Infrastructure.Utils
{
    public static class Util
    {
        private static HashSet<string> GetNonExistingFiles(List<string> files, List<string> Exists)
        {
             return files.Except(Exists) as HashSet<string>;

        }
    }
}
