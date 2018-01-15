using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConventionalAndWhiteTariffCalculator.Infraestructure;
using FluentValidation;

namespace ConventionalAndWhiteTariffCalculator.Features.Calculate
{
    public class Calculate
    {
        public class Command : IRequest<Result>
        {
            public Guid ConcessionaryId { get; set; }
            public string Name { get; set; }
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Power).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                RuleFor(c => c.Quantity).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
            }
        }

        public class Result
        {
            public Guid ProductId { get; set; }
            public TimeSpan TimeOfUse { get; set; }
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
                var tariffDetail = await TariffUtil.AllTariffCalc(db, req.ConcessionaryId, req.Power, req.Quantity, req.UseOfMonth);

                return new Result()
                {
                    ProductId = Guid.NewGuid(),
                    ConventionalTariffEnergySpending = tariffDetail.ConventionalTariffValue,
                    WhiteTariffEnergySpending = tariffDetail.WhiteTariffValue,
                    TimeOfUse = tariffDetail.TimeOfUse
                };
            }
        }
    }
}
