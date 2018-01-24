using FluentValidation;
using MediatR;
using System;
using System.Threading.Tasks;
using ConventionalAndWhiteTariffCalculatorAPI.Infraestructure;

namespace ConventionalAndWhiteTariffCalculatorAPI.Features.Tariff
{
    public partial class RegisterTariff
    {
        public class Command : IRequest<Result>
        {
            public string ConcessionaryName { get; set; }
            public TariffRange ConventionalTariff { get; set; }
            public TariffRange OffPeackI { get; set; }
            public TariffRange IntermediateI { get; set; }
            public TariffRange OnPeack { get; set; }
            public TariffRange IntermediateII { get; set; }
            public TariffRange OffPeackII { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.ConcessionaryName).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(c => c.ConventionalTariff).Custom((tariff, context) =>
                {
                    if (tariff == null) context.AddFailure("A tarifa convencional não pode ser nula");
                    else
                    {
                        if (tariff.BaseValue < 0) context.AddFailure("Valor Base da tarifa convencional deve ser maior que zero");

                        if (!tariff.TimeInit.Equals(new TimeSpan(00, 00, 00))) context.AddFailure("Horário de Início deve 00:00:00");

                        if (!tariff.TimeFinish.Equals(new TimeSpan(23, 59, 59))) context.AddFailure("Horário de Término deve 23:59:59");
                    }
                });
                RuleFor(c => c.OffPeackI).Custom((tariff, context) =>
                {
                    if (tariff == null) context.AddFailure("A tarifa no horário fora de ponta I não pode ser nulo");
                    else
                    {
                        if (tariff.BaseValue < 0) context.AddFailure("Valor Base do horário fora de ponta I deve ser maior que zero");

                        if (tariff.TimeFinish < tariff.TimeInit) context.AddFailure("Hora de início no período fora de ponta I deve ser menor que hora de término");
                    }
                });
                RuleFor(c => c.IntermediateI).Custom((tariff, context) =>
                {
                    if (tariff == null) context.AddFailure("A tarifa no horário intermediário I não pode ser nulo");
                    else
                    {
                        if (tariff.BaseValue < 0) context.AddFailure("Valor Base do horário intermediário I deve ser maior que zero");

                        if (tariff.TimeFinish < tariff.TimeInit) context.AddFailure("Hora de início no período intermediário I deve ser menor que hora de término");
                    }
                });
                RuleFor(c => c.OnPeack).Custom((tariff, context) =>
                {
                    if (tariff == null) context.AddFailure("A tarifa no horário de ponta não pode ser nulo");
                    else
                    {
                        if (tariff.BaseValue < 0) context.AddFailure("Valor Base do horário de ponta deve ser maior que zero");

                        if (tariff.TimeFinish < tariff.TimeInit) context.AddFailure("Hora de início no período de ponta deve ser menor que hora de término");
                    }
                });
                RuleFor(c => c.IntermediateII).Custom((tariff, context) =>
                {
                    if (tariff == null) context.AddFailure("A tarifa no horário intermediário II não pode ser nulo");
                    else
                    {
                        if (tariff.BaseValue < 0) context.AddFailure("Valor Base do horário intermediário II deve ser maior que zero");

                        if (tariff.TimeFinish < tariff.TimeInit) context.AddFailure("Hora de início no período intermediário II deve ser menor que hora de término");
                    }
                });
                RuleFor(c => c.OffPeackII).Custom((tariff, context) =>
                {
                    if (tariff == null) context.AddFailure("A tarifa no horário fora de ponta II não pode ser nulo");
                    else
                    {
                        if (tariff.BaseValue < 0) context.AddFailure("Valor Base do horário fora de ponta II deve ser maior que zero");

                        if (tariff.TimeFinish < tariff.TimeInit) context.AddFailure("Hora de início no período fora de ponta II deve ser menor que hora de término");
                    }
                });
            }
        }

        public class Result
        {
            public PowerDistribuitorAux PowerDistribuitor { get; set; }
            public TariffsAux Tariffs { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Command command)
            {
                var powerDistribuitor = new Domain.PowerDistribuitor()
                {
                    Name = command.ConcessionaryName
                };

                await db.PowerDistribuitor.AddAsync(powerDistribuitor);
                await db.SaveChangesAsync();

                var conventional = new Domain.Tariff()
                {
                    PowerDistribuitorId = powerDistribuitor.Id,
                    Name = "Conventional",
                    TariffType = "ConventionalTariff",
                    InitTime = command.ConventionalTariff.TimeInit,
                    FinishTime = command.ConventionalTariff.TimeFinish,
                    BaseValue = command.ConventionalTariff.BaseValue
                };

                var offPeackI = new Domain.Tariff()
                {
                    PowerDistribuitorId = powerDistribuitor.Id,
                    Name = "OffPeackI",
                    TariffType = "WhiteTariff",
                    InitTime = command.OffPeackI.TimeInit,
                    FinishTime = command.OffPeackI.TimeFinish,
                    BaseValue = command.OffPeackI.BaseValue
                };

                var intermediateI = new Domain.Tariff()
                {
                    PowerDistribuitorId = powerDistribuitor.Id,
                    Name = "IntermediateI",
                    TariffType = "WhiteTariff",
                    InitTime = command.IntermediateI.TimeInit,
                    FinishTime = command.IntermediateI.TimeFinish,
                    BaseValue = command.IntermediateI.BaseValue
                };

                var onPeack = new Domain.Tariff()
                {
                    PowerDistribuitorId = powerDistribuitor.Id,
                    Name = "OnPeack",
                    TariffType = "WhiteTariff",
                    InitTime = command.OnPeack.TimeInit,
                    FinishTime = command.OnPeack.TimeFinish,
                    BaseValue = command.OnPeack.BaseValue
                };

                var intermediateII = new Domain.Tariff()
                {
                    PowerDistribuitorId = powerDistribuitor.Id,
                    Name = "IntermediateII",
                    TariffType = "WhiteTariff",
                    InitTime = command.IntermediateII.TimeInit,
                    FinishTime = command.IntermediateII.TimeFinish,
                    BaseValue = command.IntermediateII.BaseValue
                };

                var offPeackII = new Domain.Tariff()
                {
                    PowerDistribuitorId = powerDistribuitor.Id,
                    Name = "OffPeackII",
                    TariffType = "WhiteTariff",
                    InitTime = command.OffPeackII.TimeInit,
                    FinishTime = command.OffPeackII.TimeFinish,
                    BaseValue = command.OffPeackII.BaseValue
                };

                await db.AddAsync(conventional);
                await db.AddAsync(offPeackI);
                await db.AddAsync(intermediateI);
                await db.AddAsync(onPeack);
                await db.AddAsync(intermediateII);
                await db.AddAsync(offPeackII);

                await db.SaveChangesAsync();

                return new Result()
                {
                    PowerDistribuitor = new PowerDistribuitorAux()
                    {
                        Id = powerDistribuitor.Id,
                        Name = powerDistribuitor.Name
                    },
                    Tariffs = new TariffsAux()
                    {
                        ConventionalTariff = new TariffRange()
                        {
                            BaseValue = conventional.BaseValue,
                            TimeInit = conventional.InitTime,
                            TimeFinish = conventional.FinishTime
                        },
                        OffPeackI = new TariffRange()
                        {
                            BaseValue = offPeackI.BaseValue,
                            TimeInit = offPeackI.InitTime,
                            TimeFinish = offPeackI.FinishTime
                        },
                        IntermediateI = new TariffRange()
                        {
                            BaseValue = intermediateI.BaseValue,
                            TimeInit = intermediateI.InitTime,
                            TimeFinish = intermediateI.FinishTime
                        },
                        OnPeack = new TariffRange()
                        {
                            BaseValue = onPeack.BaseValue,
                            TimeInit = onPeack.InitTime,
                            TimeFinish = onPeack.FinishTime
                        },
                        IntermediateII = new TariffRange()
                        {
                            BaseValue = intermediateII.BaseValue,
                            TimeInit = intermediateII.InitTime,
                            TimeFinish = intermediateII.FinishTime
                        },
                        OffPeackII = new TariffRange()
                        {
                            BaseValue = offPeackII.BaseValue,
                            TimeInit = offPeackII.InitTime,
                            TimeFinish = offPeackII.FinishTime
                        }
                    }
                };
            }
        }
    }
}
