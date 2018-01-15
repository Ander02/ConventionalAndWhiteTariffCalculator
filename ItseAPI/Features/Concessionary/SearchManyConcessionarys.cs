﻿using ConventionalAndWhiteTariffCalculator.Infraestructure;
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
    public class SearchManyConcessionarys
    {
        public class Query : IRequest<List<Result>>
        {
            public string Name { get; set; } = "";
            public int Limit { get; set; } = 20;
        }

        public class Result
        {
            public string Name { get; set; }
            public string City { get; set; }
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

                var q = db.Concessionary
                    .Where(c => c.Name.Contains(query.Name))
                    .OrderBy(c => c.Name)
                    .Take(query.Limit)
                    .AsQueryable();

                return (await q.ToListAsync()).Select(c => new Result
                {
                    Name = c.Name,
                    City = c.City
                }).ToList();
            }
        }
    }
}
