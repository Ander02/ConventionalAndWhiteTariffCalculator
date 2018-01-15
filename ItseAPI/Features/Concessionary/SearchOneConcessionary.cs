using ConventionalAndWhiteTariffCalculator.Infraestructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Features.Concessionary
{
    public class SearchOneConcessionary
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public string Name { get; set; }
            public string City { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task<Result> Handle(Query query)
            {
                var c = await db.Concessionary.FindAsync(query.Id);

                return new Result()
                {
                    Name = c.Name,
                    City = c.City
                };
            }
        }
    }
}
