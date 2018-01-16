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

        [HttpPatch]
        [Route("{id}")]
        public async Task<UpdateCalc.Result> UpdateCalc([FromRoute] Guid id, [FromBody] UpdateCalc.Command value)
        {
            value.Id = id;
            var result = await mediator.Send(value);

            return result;
        }
    }
}
