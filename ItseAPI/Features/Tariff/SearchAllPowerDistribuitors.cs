using ConventionalAndWhiteTariffCalculator.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Features.Tariff
{
    public class SearchAllPowerDistribuitors
    {
        public class Query : IRequest<List<Result>>
        {

        }

        public class Result
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, List<Result>>
        {
            private readonly Db db;

            public Handler(Db db)
            {
                this.db = db;
            }

            public async Task<List<Result>> Handle(Query query)
            {
                var q = db.PowerDistribuitor
                    .OrderBy(p => p.Name)
                    .AsQueryable();

                return (await q.ToListAsync()).Select(d => new Result
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
            }
        }
    }
}
