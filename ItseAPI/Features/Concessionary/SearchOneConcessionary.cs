using ConventionalAndWhiteTariffCalculator.Infraestructure;
using MediatR;
using System;
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
            public Guid Id { get; set; }
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
                    Id = c.Id,
                    Name = c.Name,
                    City = c.City
                };
            }
        }
    }
}
