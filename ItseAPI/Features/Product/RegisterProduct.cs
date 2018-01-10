using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItseAPI.Infraestructure;

namespace ItseAPI.Features.Product
{
    public class RegisterProduct
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
                var product = new Domain.Product()
                {
                    Name = command.Name,
                    DefaultPower = command.DefaultPower
                };

                await db.Product.AddAsync(product);
                await db.SaveChangesAsync();

                return new Result()
                {
                    Id = product.Id,
                    Name = product.Name,
                    DefaultPower = product.DefaultPower
                };
            }
        }
    }
}
