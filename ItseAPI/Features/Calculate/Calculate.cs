using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItseAPI.Infraestructure;

namespace ItseAPI.Features.Calculate
{
    public class Calculate
    {
        public class DateInitAndFinish
        {
            public DateTime DateTimeInit { get; set; }
            public DateTime DateTimeFinish { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
            public double TotalMitutes { get; set; }
            public double WhiteTariffEnergySpending { get; set; }
            public double ConventionalTariffEnergySpending { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Command req)
            {
                return new Result()
                {
                    Id = Guid.NewGuid(),
                    Name = req.Name,
                    Power = req.Power,
                    Quantity = req.Quantity,
                    UseOfMonth = req.UseOfMonth,
                    ConventionalTariffEnergySpending = await TariffUtil.ConventionalTariffCalc(db, req),
                    WhiteTariffEnergySpending = await TariffUtil.WhiteTariffCalc(db, req),
                    TotalMitutes = await TariffUtil.TotalMinutes(db, req.UseOfMonth)
                };
            }
        }
    }
}
