using lojaService;
using lojaServiceContract;
using lojaServiceContract.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace servicesTests
{
    public class LojaServiceTest
    {

        public ServiceCollection Services { get; private set; }
        public ServiceProvider ServiceProvider { get; protected set; }

        public LojaServiceTest()
        {
            Services = new ServiceCollection();

            Services.AddDbContext<LojaContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryDb"), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            Services.AddScoped<ILojaService, LojaService>();

            ServiceProvider = Services.BuildServiceProvider();

            using (var serviceScope = ServiceProvider.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<LojaContext>();
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task Upload()
        {
            var LojaService = ServiceProvider.GetService<ILojaService>();

            var fileLines = new List<string>();

            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            var filePath = Path.Combine(dirPath, "CNAB.txt");

            foreach (string line in System.IO.File.ReadLines(filePath))
            {
                fileLines.Add(line);
            }

            await LojaService.ProcessarTransacoes(new ProcessarTransacoesRequest()
            {
                TransacaoLinha = fileLines
            });

            var listarLojasResponse = await LojaService.ListarLojas(new lojaServiceContract.DTO.ListarLojasRequest());


            var barDoJoao = listarLojasResponse.Lojas.First(x => x.Nome.Trim() == "BAR DO JOÃO");

            var listarTransacoesResponseBarDoJoao = await LojaService.ListarTransacoes(new ListarTransacoesRequest()
            {
                LojaId = (int)barDoJoao.Id
            });

            Assert.Equal(-102, listarTransacoesResponseBarDoJoao.Saldo);


            var marcosDaAvenida = listarLojasResponse.Lojas.First(x => x.Nome.Trim() == "MERCADO DA AVENIDA");

            var listarTransacoesResponseMarcosDaAvenida = await LojaService.ListarTransacoes(new ListarTransacoesRequest()
            {
                LojaId = (int)marcosDaAvenida.Id
            });

            Assert.Equal(489.2M, listarTransacoesResponseMarcosDaAvenida.Saldo);
        }
    }
}
