using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace ConventionalAndWhiteTariffCalculator.Features.Equipament
{
    [Route("/[controller]")]
    public class EquipamentsController : Controller
    {
        private IMediator mediator;

        public EquipamentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterEquipament([FromBody] RegisterEquipament.Command value)
        {
            var result = await mediator.Send(value);

            return Created(this.Request.Path.Value + "/" + result.Id, result);
        }

        [HttpGet]
        public async Task<List<SearchManyEquipaments.Result>> SearchManyEquipaments([FromQuery] SearchManyEquipaments.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<SearchOneEquipament.Result> SearchOneEquipament([FromRoute] SearchOneEquipament.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }
    }
}
