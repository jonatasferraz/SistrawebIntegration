using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IntegracaoSitraWeb.Core;
using IntegracaoSitraWeb.Infrastructure;
using IntegracaoSitraWeb.Infrastructure.Persistence;

namespace IntegracaoSitraWeb.Application.Boticario
{
    public class BoticarioReadToTms
    {
        private DiaslogNewContext _context;
        private Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.ArquivoEntradum> _RepArquivo;
        private Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.Ordem> _RepOrdem;



        public BoticarioReadToTms(DiaslogNewContext contexto)
        {
                _context = contexto;
                //_RepArquivo = new Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.ArquivoEntradum>(_context);
                //_RepOrdem = new Infrastructure.Persistence.GenericRepository<Core.Domain.Entities.Ordem>(_context);
        }
        public Task Processar()
       {
            return Task.CompletedTask;
       }

    }
}

