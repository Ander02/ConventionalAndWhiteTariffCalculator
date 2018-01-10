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
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMounth { get; set; }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMounth { get; set; }
            public long TotalMitutes { get; set; }
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

            public Task<Result> Handle(Command req)
            {
                var conventionalTariff = TariffAuxMethods.CurrentTariffCalc(req);
                var whiteTariff = TariffAuxMethods.WhiteTariffCalc(req);

                return null;
            }
        }




    }
}
