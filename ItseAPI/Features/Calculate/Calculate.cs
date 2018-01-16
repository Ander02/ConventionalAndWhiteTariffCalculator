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
                RuleFor(c => c.ConcessionaryId).NotEmpty().NotNull();
                RuleFor(c => c.Power).NotEmpty().NotNull().GreaterThanOrEqualTo(0);
                RuleFor(c => c.Quantity).NotEmpty().NotNull().GreaterThanOrEqualTo(0);

                RuleFor(c => c.UseOfMonth).Custom((list, context) =>
                {
                    if (list == null)
                    {
                        context.AddFailure("Lista com datas e horários deve ser inicializada");
                    }
                    else foreach (var item in list)
                        {
                            if (item.DateInit > item.DateFinish)
                            {
                                context.AddFailure("Data de início deve ser antes da data de fim");
                            }
                            if (item.TimeInit > item.TimeFinish)
                            {
                                context.AddFailure("Hora de início deve ser antes da Hora de fim");
                            }
                        }
                });
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
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
                    Id = Guid.NewGuid(),
                    ConventionalTariffEnergySpending = tariffDetail.ConventionalTariffValue,
                    WhiteTariffEnergySpending = tariffDetail.WhiteTariffValue,
                    TimeOfUse = tariffDetail.TimeOfUse
                };
            }
        }
    }
}
