using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConventionalAndWhiteTariffCalculatorAPI.Infraestructure;
using FluentValidation;
using System.Linq;
using ConventionalAndWhiteTariffCalculatorAPI.Util;

namespace ConventionalAndWhiteTariffCalculatorAPI.Features.Calculate
{
    public class Calculate
    {
        public class Command : IRequest<List<Result>>
        {
            public Guid PowerDistribuitorId { get; set; }
            public int Month { get; set; }
            public List<EquipmentUse> Equipments { get; set; }
        }

        public class EquipmentUse
        {
            public double? Power { get; set; }
            public int? Quantity { get; set; }
            public List<DateInitAndFinish> UseOfMonth { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.PowerDistribuitorId).NotEmpty().NotNull();
                RuleFor(c => c.Month).NotEmpty().NotNull().GreaterThan(0).LessThanOrEqualTo(12);

                RuleFor(c => c).NotNull().Custom((command, context) =>
                {
                    if (command.Equipments == null) context.AddFailure("Lista com equipamentos não pode ser nula");

                    else foreach (var equip in command.Equipments)
                        {
                            if (equip.Power == null || equip.Power.Value <= 0) context.AddFailure("Potência de um dos equipamentos não é válido");

                            if (equip.Quantity == null || equip.Quantity.Value <= 0) context.AddFailure("Quantidade de um dos equipamentos não é válido");

                            if (equip.UseOfMonth == null) context.AddFailure("Lista com datas e horários não pode ser nula");

                            else foreach (var date in equip.UseOfMonth)
                                {
                                    if (date.TimeInit.HasValue)
                                    {
                                        if (date.TimeInit.Value < new TimeSpan(0, 0, 0)) context.AddFailure("Hora de início deve ser positiva");
                                        if (date.TimeInit.Value > new TimeSpan(23, 59, 59)) context.AddFailure("Hora de início inválida");
                                    }
                                    else context.AddFailure("Hora de início não pode ser nula");

                                    if (date.TimeFinish.HasValue)
                                    {
                                        if (date.TimeFinish.Value < new TimeSpan(0, 0, 0)) context.AddFailure("Hora de fim deve ser positiva");
                                        if (date.TimeFinish.Value > new TimeSpan(23, 59, 59)) context.AddFailure("Hora de fim inválida");
                                    }
                                    else context.AddFailure("Hora de fim não pode ser nula");

                                    if (date.DateInit.HasValue)
                                    {
                                        if (date.DateInit.Value.Month.NotEquals(command.Month)) context.AddFailure("Data de início contém mês inválido");

                                    }
                                    else context.AddFailure("Data de início não pode ser nula");

                                    if (date.DateFinish.HasValue)
                                    {
                                        if (date.DateFinish.Value.Month.NotEquals(command.Month)) context.AddFailure("Data de fim contém mês inválido");
                                    }
                                    else context.AddFailure("Data de fim não pode ser nula");

                                    if (date.DateInit.HasValue && date.DateInit.HasValue)
                                    {
                                        if (date.DateInit.Value > date.DateFinish.Value) context.AddFailure("Data de início deve ser antes da data de fim");
                                    }

                                    if (date.TimeInit.HasValue && date.TimeInit.HasValue)
                                    {
                                        if (date.TimeInit.Value > date.TimeFinish.Value) context.AddFailure("Hora de início deve ser antes da Hora de fim");
                                    }
                                }
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

        public class Handler : IAsyncRequestHandler<Command, List<Result>>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task<List<Result>> Handle(Command req)
            {
                var resultList = new List<Result>();
                foreach (var item in req.Equipments)
                {
                    var tariffDetail = await TariffUtil.AllTariffCalc(db, req.PowerDistribuitorId, item.Power.Value, item.Quantity.Value, item.UseOfMonth);
                    resultList.Add(new Result()
                    {
                        TimeOfUse = tariffDetail.TimeOfUse,
                        ConventionalTariffEnergySpending = Math.Round(tariffDetail.ConventionalTariffValue, 2),
                        WhiteTariffEnergySpending = Math.Round(tariffDetail.WhiteTariffValue, 2)
                    });
                }

                return resultList;
            }
        }
    }
}
