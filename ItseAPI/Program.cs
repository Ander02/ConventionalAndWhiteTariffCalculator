using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ConventionalAndWhiteTariffCalculatorAPI.Infraestructure;
using Microsoft.Extensions.DependencyInjection;

namespace ConventionalAndWhiteTariffCalculatorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool runSeed = false;

            if (args.Contains("seed"))
            {
                runSeed = true;
                args = args.Where(d => d != "seed").ToArray();
            }

            var host = BuildWebHost(args);

            //Executa com o Seed
            if (runSeed) RunSeed(host).Wait();

            //Executa normalmente
            else host.Run();
        }

        private static async Task RunSeed(IWebHost host)
        {
            Console.WriteLine("Running seed...");
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Startup>>();
                logger.LogInformation("Seed log services acquired");
                var dbContext = services.GetService<Db>();
                logger.LogInformation("DataBase context acquired");
                try
                {
                    //Inicializa o Banco de Dados
                    await DbInitializer.Initialize(dbContext, logger);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.GetBaseException().Message);
                    logger.LogError(ex, "An error occurred while seeding the database!!");
                }
                finally
                {
                    Console.WriteLine("Seed ended");
                    Console.ReadKey();
                }
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
