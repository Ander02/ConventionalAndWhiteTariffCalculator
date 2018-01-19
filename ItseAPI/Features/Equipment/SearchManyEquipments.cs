using ConventionalAndWhiteTariffCalculator.Infraestructure;
using ConventionalAndWhiteTariffCalculatorAPI.Util;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Features.Equipment
{
    public class SearchManyEquipments
    {
        public class Query : IRequest<List<Result>>
        {
            public string Name { get; set; } = "";
            public int Limit { get; set; } = 20;
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.Limit).NotNull().GreaterThanOrEqualTo(0);
            }
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

                var equips = await db.Equipament
                    .OrderBy(p => p.Name)
                    .Take(query.Limit)
                    .ToListAsync();

                var result = equips.Where(p => p.Name.RemoveAcentuation().ToLower().Contains(query.Name.RemoveAcentuation().ToLower())).ToList();

                return result.Select(p => new Result
                {
                    Name = p.Name,
                    DefaultPower = p.DefaultPower
                }).ToList();
            }
        }
    }
}
