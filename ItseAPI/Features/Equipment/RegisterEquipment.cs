using FluentValidation;
using MediatR;
using System;
using System.Threading.Tasks;
using ConventionalAndWhiteTariffCalculator.Infraestructure;

namespace ConventionalAndWhiteTariffCalculator.Features.Equipment
{
    public class RegisterEquipment
    {
        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
            public double DefaultPower { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(c => c.DefaultPower).NotNull().NotEmpty().GreaterThanOrEqualTo(0);
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public double DefaultPower { get; set; }
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
                var equipment = new Domain.Equipment()
                {
                    Name = command.Name,
                    DefaultPower = command.DefaultPower
                };

                await db.Equipament.AddAsync(equipment);
                await db.SaveChangesAsync();

                return new Result()
                {
                    Id = equipment.Id,
                    Name = equipment.Name,
                    DefaultPower = equipment.DefaultPower
                };
            }
        }
    }
}
