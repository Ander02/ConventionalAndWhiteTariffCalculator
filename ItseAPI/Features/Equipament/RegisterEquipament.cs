using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConventionalAndWhiteTariffCalculator.Infraestructure;

namespace ConventionalAndWhiteTariffCalculator.Features.Equipament
{
    public class RegisterEquipament
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
                RuleFor(c => c.DefaultPower).NotNull().NotEmpty().GreaterThan(0);
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
                var Equipament = new Domain.Equipament()
                {
                    Name = command.Name,
                    DefaultPower = command.DefaultPower
                };

                await db.Equipament.AddAsync(Equipament);
                await db.SaveChangesAsync();

                return new Result()
                {
                    Id = Equipament.Id,
                    Name = Equipament.Name,
                    DefaultPower = Equipament.DefaultPower
                };
            }
        }
    }
}
