using ConventionalAndWhiteTariffCalculatorAPI.Infraestructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculatorAPI.Features.Tariff
{
    public class SearchManyTariffs
    {
        public class Query : IRequest<List<Result>>
        {
            public string Name { get; set; } = "";
        }

        public class Result
        {
            public PowerDistribuitorAux PowerDistribuitor { get; set; }
            public List<TariffDetails> Tariffs { get; set; }
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

                var q = db.PowerDistribuitor
                    .Where(p => p.Name.Contains(query.Name))
                    .Include(p => p.Tariffs)
                    .OrderBy(p => p.Name)
                    .AsQueryable();

                return (await q.ToListAsync()).Select(d => new Result
                {
                    PowerDistribuitor = new PowerDistribuitorAux()
                    {
                        Id = d.Id,
                        Name = d.Name
                    },
                    Tariffs = d.Tariffs.Select(t => new TariffDetails()
                    {
                        Name = t.Name,
                        TariffType = t.TariffType,
                        BaseValue = t.BaseValue,
                        TimeFinish = t.FinishTime,
                        TimeInit = t.InitTime
                    }).ToList()
                }).ToList();
            }
        }
    }
}
