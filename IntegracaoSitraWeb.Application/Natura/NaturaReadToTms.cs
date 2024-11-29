using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegracaoSitraWeb.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IntegracaoSitraWeb.Application.Natura
{
    public class NaturaReadToTms
    {
        private DiaslogNewContext _context;
        private Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.ArquivoEntradum> _RepArquivo;
        private Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.Ordem> _RepOrdem;
        private readonly string _origem = "Natura";

        public NaturaReadToTms(DiaslogNewContext contexto)
        {
                _context = contexto;
                _RepArquivo = new Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.ArquivoEntradum>(_context);
                _RepOrdem = new Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.Ordem>(_context);
        }


        public Task Processar()
        {
            return Task.CompletedTask;
        }

      
    }
}
