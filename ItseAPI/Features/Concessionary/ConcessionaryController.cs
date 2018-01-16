using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace ConventionalAndWhiteTariffCalculator.Features.Concessionary
{
    [Route("/[controller]")]
    public class ConcessionaryController : Controller
    {
        private IMediator mediator;

        public ConcessionaryController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterConcessionary([FromBody] RegisteConcessionary.Command value)
        {
            var result = await mediator.Send(value);

            return Created(this.Request.Path.Value + "/" + result.Id, result);
        }

        [HttpGet]
        public async Task<List<SearchManyConcessionarys.Result>> SearchManyConcessionarys([FromQuery] SearchManyConcessionarys.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<SearchOneConcessionary.Result> SearchOneConcessionary([FromRoute] SearchOneConcessionary.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }
    }
}
