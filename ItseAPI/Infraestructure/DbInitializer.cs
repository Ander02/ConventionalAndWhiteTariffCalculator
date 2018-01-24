using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ConventionalAndWhiteTariffCalculatorAPI.Domain;

namespace ConventionalAndWhiteTariffCalculatorAPI.Infraestructure
{
    public class DbInitializer
    {
        public static async Task Initialize(Db db, ILogger<Startup> logger)
        {
            ////Inicializa o Banco
            //logger.LogInformation("Starting database");

            //await db.PowerDistribuitor.AddAsync(new PowerDistribuitor()
            //{
            //    Name = "AES Eletropaulo"
            //});

            //await db.SaveChangesAsync();

            //var distribuitor = db.PowerDistribuitor.Where(c => c.Name.Contains("AES Eletropaulo")).FirstOrDefault();

            //await db.Tariff.AddAsync(new Tariff()
            //{
            //    PowerDistribuitorId = distribuitor.Id,
            //    Name = "ConventionalTariff",
            //    TariffType = "Conventional",
            //    InitTime = new TimeSpan(00, 00, 00),
            //    FinishTime = new TimeSpan(23, 59, 59),
            //    BaseValue = 419.61
            //});

            //await db.Tariff.AddAsync(new Tariff()
            //{
            //    PowerDistribuitorId = distribuitor.Id,
            //    Name = "OffPeackI",
            //    TariffType = "WhiteTariff",
            //    InitTime = new TimeSpan(00, 00, 00),
            //    FinishTime = new TimeSpan(16, 29, 59),
            //    BaseValue = 353.25
            //});

            //await db.Tariff.AddAsync(new Tariff()
            //{
            //    PowerDistribuitorId = distribuitor.Id,
            //    Name = "IntermediateI",
            //    TariffType = "WhiteTariff",
            //    InitTime = new TimeSpan(16, 30, 00),
            //    FinishTime = new TimeSpan(17, 29, 59),
            //    BaseValue = 496.66
            //});

            //await db.Tariff.AddAsync(new Tariff()
            //{
            //    PowerDistribuitorId = distribuitor.Id,
            //    Name = "OnPeack",
            //    TariffType = "WhiteTariff",
            //    InitTime = new TimeSpan(17, 30, 00),
            //    FinishTime = new TimeSpan(20, 29, 59),
            //    BaseValue = 760.21
            //});

            //await db.Tariff.AddAsync(new Tariff()
            //{
            //    PowerDistribuitorId = distribuitor.Id,
            //    Name = "IntermediateII",
            //    TariffType = "WhiteTariff",
            //    InitTime = new TimeSpan(20, 30, 00),
            //    FinishTime = new TimeSpan(21, 29, 59),
            //    BaseValue = 496.66
            //});

            //await db.Tariff.AddAsync(new Tariff()
            //{
            //    PowerDistribuitorId = distribuitor.Id,
            //    Name = "OffPeackII",
            //    TariffType = "WhiteTariff",
            //    InitTime = new TimeSpan(21, 30, 00),
            //    FinishTime = new TimeSpan(23, 59, 59),
            //    BaseValue = 353.25
            //});

            await db.SaveChangesAsync();
        }
    }
}
