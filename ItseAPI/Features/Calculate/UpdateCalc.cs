using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItseAPI.Infraestructure;
using FluentValidation;

namespace ItseAPI.Features.Calculate
{
    public class UpdateCalc
    {
        public class Command : IRequest<Result>
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Id).NotEmpty().NotNull();
                RuleFor(c => c.Power).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                RuleFor(c => c.Quantity).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public double Power { get; set; }
            public int Quantity { get; set; }
            public TimeSpan TimeOfUse { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
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
                    Id = req.Id,
                    Name = req.Name,
                    Power = req.Power,
                    Quantity = req.Quantity,
                    UseOfMonth = req.UseOfMonth,
                    ConventionalTariffEnergySpending = await TariffUtil.ConventionalTariffCalc(db, req.Power, req.Quantity, req.UseOfMonth),
                    WhiteTariffEnergySpending = await TariffUtil.WhiteTariffCalc(db, req.Power, req.Quantity, req.UseOfMonth),
                    TimeOfUse = TariffUtil.TotalTime(req.UseOfMonth)
                };
            }
        }
    }
}
