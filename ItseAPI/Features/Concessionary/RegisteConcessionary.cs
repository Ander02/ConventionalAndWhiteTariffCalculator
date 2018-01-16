using FluentValidation;
using MediatR;
using System;
using System.Threading.Tasks;
using ConventionalAndWhiteTariffCalculator.Infraestructure;

namespace ConventionalAndWhiteTariffCalculator.Features.Concessionary
{
    public class RegisteConcessionary
    {
        public class Command : IRequest<Result>
        {
            public string Name { get; set; }
            public string City { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.Name).NotNull().NotEmpty().MaximumLength(50);
                RuleFor(c => c.City).NotNull().NotEmpty().MaximumLength(50);
            }
        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
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
                var concessionary = new Domain.Concessionary()
                {
                    Name = command.Name,
                    City = command.City
                };

                await db.Concessionary.AddAsync(concessionary);
                await db.SaveChangesAsync();

                return new Result()
                {
                    Id = concessionary.Id,
                    Name = concessionary.Name,
                    City = concessionary.City
                };
            }
        }
    }
}
