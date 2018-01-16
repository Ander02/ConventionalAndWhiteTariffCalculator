using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace ConventionalAndWhiteTariffCalculator.Features.Tariff
{
    [Route("/[controller]")]
    public class TariffsController : Controller
    {
        private IMediator mediator;

        public TariffsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterTariff([FromBody] RegisteTariff.Command value)
        {
            var result = await mediator.Send(value);

            return Created(this.Request.Path.Value + "/" + result.PowerDistribuitor.Id, result);
        }

        [HttpGet]
        [Route("/powerdistribuitor")]
        public async Task<List<SearchManyPowerDistribuitors.Result>> SearchManyPowerDistribuitor([FromQuery] SearchManyPowerDistribuitors.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }

        [HttpGet]
        public async Task<List<SearchManyTariffs.Result>> SearchManyTariffs([FromQuery] SearchManyTariffs.Query query)
        {
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
