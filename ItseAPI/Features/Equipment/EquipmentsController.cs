using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace ConventionalAndWhiteTariffCalculator.Features.Equipment
{
    [Route("/[controller]")]
    public class EquipmentsController : Controller
    {
        private IMediator mediator;

        public EquipmentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /*
            [HttpPost]
            public async Task<ActionResult> RegisterEquipament([FromBody] RegisterEquipment.Command value)
            {
                var result = await mediator.Send(value);

                return Created(this.Request.Path.Value + "/" + result.Id, result);
            }
        */

        [HttpGet]
        public async Task<List<SearchManyEquipments.Result>> SearchManyEquipments([FromQuery] SearchManyEquipments.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<SearchOneEquipment.Result> SearchOneEquipment([FromRoute] SearchOneEquipment.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }
    }
}
