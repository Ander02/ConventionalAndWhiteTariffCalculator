using ItseAPI.Features;
using ItseAPI.Features.Calculate;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ItseAPI.Features.Equipament
{
    [Route("api/[controller]")]
    public class EquipamentController : Controller
    {
        private IMediator mediator;

        public EquipamentController(IMediator mediator)
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
