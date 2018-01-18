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
            public Guid PowerDistribuitorId { get; set; }
            public double Power { get; set; }
            public int Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PowerDistribuitorId).NotEmpty().NotNull();
                RuleFor(c => c.Power).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                RuleFor(c => c.Quantity).NotEmpty().NotNull().GreaterThanOrEqualTo(0);

                RuleFor(c => c.UseOfMonth).Custom((list, context) =>
                {
                    if (list == null) context.AddFailure("Lista com datas e horários não pode ser nula");

                    else foreach (var item in list)
                        {
                            if (item.TimeInit < new TimeSpan(0,0,0)) context.AddFailure("Hora de início deve ser positiva");

                            if (item.TimeFinish < new TimeSpan(0, 0, 0)) context.AddFailure("Hora de fim deve ser positiva");

                            if (item.DateInit > item.DateFinish) context.AddFailure("Data de início deve ser antes da data de fim");

                            if (item.TimeInit > item.TimeFinish) context.AddFailure("Hora de início deve ser antes da Hora de fim");
                        }
                });
            }
        }

        public class Result
        {
            public string TimeOfUse { get; set; }
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
                var tariffDetail = await TariffUtil.AllTariffCalc(db, req.PowerDistribuitorId, req.Power, req.Quantity, req.UseOfMonth);

                return new Result()
                {
                    ConventionalTariffEnergySpending = tariffDetail.ConventionalTariffValue,
                    WhiteTariffEnergySpending = tariffDetail.WhiteTariffValue,
                    TimeOfUse = tariffDetail.TimeOfUse
                };
            }
        }
    }
}
