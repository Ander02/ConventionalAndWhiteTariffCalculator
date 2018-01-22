using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConventionalAndWhiteTariffCalculator.Infraestructure;
using FluentValidation;
using ConventionalAndWhiteTariffCalculatorAPI.Util;

namespace ConventionalAndWhiteTariffCalculator.Features.Calculate
{
    public class Calculate
    {
        public class Command : IRequest<Result>
        {
            public Guid PowerDistribuitorId { get; set; }
            public int Month { get; set; }
            public double? Power { get; set; }
            public int? Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PowerDistribuitorId).NotEmpty().NotNull();
                RuleFor(c => c.Power).NotNull().GreaterThanOrEqualTo(0);
                RuleFor(c => c.Month).NotEmpty().NotNull().GreaterThan(0).LessThanOrEqualTo(12);

                RuleFor(c => c.Quantity).NotNull().GreaterThanOrEqualTo(0);

                RuleFor(c => new { c.UseOfMonth, c.Month }).NotNull().Custom((obj, context) =>
                 {
                     if (obj.UseOfMonth == null) context.AddFailure("Lista com datas e horários não pode ser nula");

                     else foreach (var item in obj.UseOfMonth)
                         {
                             if (item.TimeInit == null) context.AddFailure("Hora de início não pode ser nula");

                             if (item.TimeFinish == null) context.AddFailure("Hora de término não pode ser nula");

                             if (item.TimeInit == null) context.AddFailure("Data de início não pode ser nula");

                             if (item.TimeFinish == null) context.AddFailure("Data de término não pode ser nula");

                             if (item.TimeInit.Value < new TimeSpan(0, 0, 0)) context.AddFailure("Hora de início deve ser positiva");

                             if (item.TimeFinish.Value < new TimeSpan(0, 0, 0)) context.AddFailure("Hora de fim deve ser positiva");

                             if (item.TimeInit.Value > new TimeSpan(23, 59, 59)) context.AddFailure("Hora de início inválida");

                             if (item.TimeFinish.Value > new TimeSpan(23, 59, 59)) context.AddFailure("Hora de fim inválida");

                             if (item.DateInit.Value > item.DateFinish.Value) context.AddFailure("Data de início deve ser antes da data de fim");

                             if (item.TimeInit.Value > item.TimeFinish.Value) context.AddFailure("Hora de início deve ser antes da Hora de fim");

                             if (item.DateInit.Value.Month.NotEquals(obj.Month)) context.AddFailure("Data de início contém mês inválido");

                             if (item.DateFinish.Value.Month.NotEquals(obj.Month)) context.AddFailure("Data de fim contém mês inválido");

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
                var tariffDetail = await TariffUtil.AllTariffCalc(db, req.PowerDistribuitorId, req.Power.Value, req.Quantity.Value, req.UseOfMonth);

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
