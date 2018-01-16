using ConventionalAndWhiteTariffCalculator.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Features.Equipament
{
    public class SearchManyEquipaments
    {
        public class Query : IRequest<List<Result>>
        {
            public string Name { get; set; } = "";
            public int Limit { get; set; } = 20;
        }

        public class Result
        {
            public string Name { get; set; }
            public double DefaultPower { get; set; }
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
                if (query.Name == null) query.Name = "";

                var q = db.Equipament
                    .Where(p => p.Name.Contains(query.Name))
                    .OrderBy(p => p.Name)
                    .Take(query.Limit)
                    .AsQueryable();

                return (await q.ToListAsync()).Select(p => new Result
                {
                    Name = p.Name,
                    DefaultPower = p.DefaultPower
                }).ToList();
            }
        }
    }
}
