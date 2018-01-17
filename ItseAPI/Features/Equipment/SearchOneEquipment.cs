using ConventionalAndWhiteTariffCalculator.Infraestructure;
using MediatR;
using System;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Features.Equipment
{
    public class SearchOneEquipment
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
                var p = await db.Equipament.FindAsync(query.Id);

                return new Result()
                {
                    Name = p.Name,
                    DefaultPower = p.DefaultPower
                };
            }
        }
    }
}
