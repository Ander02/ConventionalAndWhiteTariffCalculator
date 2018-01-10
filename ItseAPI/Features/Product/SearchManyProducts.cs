using ItseAPI.Infraestructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItseAPI.Features.Product
{
    public class SearchManyProducts
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
                var q = db.Product
                    .Where(p => p.Name.Contains(query.Name))
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
