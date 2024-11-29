using IntegracaoSitraWeb.Application.Boticario;
using IntegracaoSitraWeb.Application.Natura;
using IntegracaoSitraWeb.Application.SendToTms;
using IntegracaoSitraWeb.Core.Domain.Entities;
using IntegracaoSitraWeb.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

internal class Program
{ 
    private static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
           .AddDbContext<DiaslogNewContext>(options =>
                options.UseLazyLoadingProxies()
                .UseSqlServer("Server=107.182.228.220,53691;Database=diaslog_new;User Id=diaslog;Password=A(W-ty7rmTm{}4Q;TrustServerCertificate=True;"))
           .AddScoped<GenericRepository<ArquivoSitraweb>>()
           .AddScoped<GenericRepository<Ordem>>()
           .AddScoped<GenericRepository<OrdemArquivoSitraweb>>()
           .AddScoped<GenericRepository<Origem>>()
           .AddScoped<GenericRepository<Volume>>()
            .AddScoped<ApplicationSendToTms>()
           .BuildServiceProvider();
        
        while (true)
        {
            var appSendToTms = serviceProvider.GetService<ApplicationSendToTms>();
            appSendToTms?.Processar();
            int gapProcessamento = 5;
            Thread.Sleep(1000 * 60 * gapProcessamento);
        }

    }


}