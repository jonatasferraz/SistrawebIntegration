using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoTms.Core.Layers;

namespace IntegracaoTms.Core.Interfaces
{
    public interface IArquivosService
    {
        List<DtoArquivos> GetArquivos();
        
        void AddArquivos(DtoArquivos arquivo);

        void UpdateArquivos(DtoArquivos arquivo);

        

    }
}
