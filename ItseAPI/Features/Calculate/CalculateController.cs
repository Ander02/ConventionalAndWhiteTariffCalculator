using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculator.Features.Calculate
{
    [Route("/[controller]")]
    public class CalculateController : Controller
    {
        private IMediator mediator;

        public CalculateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<Calculate.Result> Calculate([FromBody] Calculate.Command value)
        {
            var result = await mediator.Send(value);

            return result;
        }
    }
}
