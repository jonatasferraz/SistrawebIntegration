using IntegracaoSitraWeb.Application;
using IntegracaoSitraWeb.Application.Boticario;
using IntegracaoSitraWeb.Application.Natura;
using IntegracaoSitraWeb.Application.ReadToTms;
using IntegracaoSitraWeb.Core.Domain.Entities;
using IntegracaoSitraWeb.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public class Program
{ 

    public static void Main(string[] args)
    {
        

        var serviceProvider = new ServiceCollection()
            .AddDbContext<DiaslogNewContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer("Server=107.182.228.220,53691;Database=nome_do_banco;User Id=diaslog;Password=A(W-ty7rmTm{}4Q;TrustServerCertificate=True;")) 
            .AddScoped<GenericRepository<ArquivoSitraweb>>()
            .AddScoped<GenericRepository<Ordem>>()
            .AddScoped<GenericRepository<OrdemArquivoSitraweb>>()
            .AddScoped<GenericRepository<Origem>>()
            .AddScoped<GenericRepository<Volume>>()
            .AddScoped<BoticarioReadToTms>()
            .AddScoped<NaturaReadToTms>()
            .AddScoped<AppReadToTms>()
            .BuildServiceProvider();

        while (true)
        {
           Console.WriteLine("Iniciando Leitura de Ordens Boticario");
            var  _BoticarioRead = serviceProvider.GetService<BoticarioReadToTms>();
            var  _NaturaRead = serviceProvider.GetService<NaturaReadToTms>();

            if (_BoticarioRead != null) _BoticarioRead.Processar();
            if (_NaturaRead != null) _NaturaRead.Processar();
            Thread.Sleep(10000);
        }
          
    }
}