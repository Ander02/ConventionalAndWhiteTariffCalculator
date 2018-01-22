using ConventionalAndWhiteTariffCalculator.Infraestructure;
using ConventionalAndWhiteTariffCalculatorAPI.Infraestructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Features.Tariff
{
    public class SearchOneTariff
    {
        public class Query : IRequest<Result>
        {
            public Guid Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.Id).NotNull().NotEmpty();
            }
        }

        public class Result
        {
            public PowerDistribuitorAux PowerDistribuitor { get; set; }
            public List<TariffDetails> Tariffs { get; set; }
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
                var powerDistribuitor = await db.PowerDistribuitor.Where(p => p.Id.Equals(query.Id)).Include(p => p.Tariffs).FirstOrDefaultAsync();

                if (powerDistribuitor == null) throw new NotFoundException("Distribuidora de energia não encontrada");

                return new Result()
                {
                    PowerDistribuitor = new PowerDistribuitorAux()
                    {
                        Id = powerDistribuitor.Id,
                        Name = powerDistribuitor.Name
                    },
                    Tariffs = powerDistribuitor.Tariffs.Select(t => new TariffDetails()
                    {
                        Name = t.Name,
                        TariffType = t.TariffType,
                        BaseValue = t.BaseValue,
                        TimeFinish = t.FinishTime,
                        TimeInit = t.InitTime
                    }).ToList()
                };
            }
        }
    }
}
