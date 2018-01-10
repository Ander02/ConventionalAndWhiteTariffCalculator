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
    public class SearchOneProduct
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class Result
        {
            public string Name { get; set; }
            public double DefaultPower { get; set; }
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
                var p = await db.Product.FindAsync(query.Id);

                return new Result()
                {
                    Name = p.Name,
                    DefaultPower = p.DefaultPower
                };
            }
        }
    }
}
