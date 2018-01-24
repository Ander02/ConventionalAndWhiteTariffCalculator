using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ConventionalAndWhiteTariffCalculatorAPI.Features.Calculate
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
