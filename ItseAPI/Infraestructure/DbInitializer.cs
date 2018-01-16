using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ConventionalAndWhiteTariffCalculator.Domain;

namespace ConventionalAndWhiteTariffCalculator.Infraestructure
{
    public class DbInitializer
    {
        public static async Task Initialize(Db db, ILogger<Startup> logger)
        {
            //Inicializa o Banco
            logger.LogInformation("Starting database");

            await db.Concessionary.AddAsync(new Concessionary()
            {
                Name = "AES Eletropaulo",
                City = "São Paulo"
            });

            await db.SaveChangesAsync();

            var concessionary = db.Concessionary.Where(c => c.Name.Contains("AES Eletropaulo")).FirstOrDefault();

            await db.Tariff.AddAsync(new Tariff()
            {
                ConcessionaryId = concessionary.Id,
                Name = "ConventionalTariff",
                TariffType = "Conventional",
                InitTime = new TimeSpan(00, 00, 00),
                FinishTime = new TimeSpan(23, 59, 59),
                BaseValue = 419.61
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                ConcessionaryId = concessionary.Id,
                Name = "OffPeackI",
                TariffType = "WhiteTariff",
                InitTime = new TimeSpan(00, 00, 00),
                FinishTime = new TimeSpan(16, 29, 59),
                BaseValue = 353.25
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                ConcessionaryId = concessionary.Id,
                Name = "IntermediateI",
                TariffType = "WhiteTariff",
                InitTime = new TimeSpan(16, 30, 00),
                FinishTime = new TimeSpan(17, 29, 59),
                BaseValue = 496.66
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                ConcessionaryId = concessionary.Id,
                Name = "OnPeack",
                TariffType = "WhiteTariff",
                InitTime = new TimeSpan(17, 30, 00),
                FinishTime = new TimeSpan(20, 29, 59),
                BaseValue = 760.21
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                ConcessionaryId = concessionary.Id,
                Name = "IntermediateII",
                TariffType = "WhiteTariff",
                InitTime = new TimeSpan(20, 30, 00),
                FinishTime = new TimeSpan(21, 29, 59),
                BaseValue = 496.66
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                ConcessionaryId = concessionary.Id,
                Name = "OffPeackII",
                TariffType = "WhiteTariff",
                InitTime = new TimeSpan(21, 30, 00),
                FinishTime = new TimeSpan(23, 59, 59),
                BaseValue = 353.25
            });

            await db.SaveChangesAsync();
        }
    }
}
