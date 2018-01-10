using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ItseAPI.Domain;

namespace ItseAPI.Infraestructure
{
    public class DbInitializer
    {
        public static async Task Initialize(Db db, ILogger<Startup> logger)
        {
            //Inicializa o Banco
            logger.LogInformation("Starting database");

            await db.Tariff.AddAsync(new Tariff()
            {
                Name = "ConventionalTariff",
                InitTime = new TimeSpan(00, 00, 00),
                FinishTime = new TimeSpan(23, 59, 59),
                BaseValue = 491.61
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                Name = "WhiteTariffOffPeackI",
                InitTime = new TimeSpan(00, 00, 01),
                FinishTime = new TimeSpan(16, 30, 00),
                BaseValue = 353.25
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                Name = "WhiteTariffIntermediateI",
                InitTime = new TimeSpan(16, 30, 01),
                FinishTime = new TimeSpan(17, 30, 00),
                BaseValue = 496.66
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                Name = "WhiteTariffOnPeack",
                InitTime = new TimeSpan(17, 30, 01),
                FinishTime = new TimeSpan(20, 30, 00),
                BaseValue = 760.21
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                Name = "WhiteTariffIntermediateII",
                InitTime = new TimeSpan(20, 30, 01),
                FinishTime = new TimeSpan(21, 30, 00),
                BaseValue = 496.66
            });

            await db.Tariff.AddAsync(new Tariff()
            {
                Name = "WhiteTariffOffPeackII",
                InitTime = new TimeSpan(21, 30, 01),
                FinishTime = new TimeSpan(00, 00, 00),
                BaseValue = 353.25
            });

            await db.SaveChangesAsync();
        }
    }
}
