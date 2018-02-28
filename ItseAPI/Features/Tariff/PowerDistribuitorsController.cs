using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace ConventionalAndWhiteTariffCalculatorAPI.Features.Tariff
{
    [Route("/[controller]")]
    public class PowerDistribuitorsController : Controller
    {
        private IMediator mediator;

        public PowerDistribuitorsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<List<SearchAllPowerDistribuitors.Result>> SearchManyPowerDistribuitor([FromQuery] SearchAllPowerDistribuitors.Query query)
        {
            //Console.Beep(new Random().Next(500,5000),750);
            var result = await mediator.Send(query);

            return result;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<SearchOneTariff.Result> SearchTariffsForPowerDistribuitor([FromRoute] SearchOneTariff.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }
    }
}
