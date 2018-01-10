using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItseAPI.Features.Calculate
{
    [Route("/api/[controller]")]
    public class CalculateController : Controller
    {
        private IMediator mediator;

        public CalculateController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Calculate([FromBody] Calculate.Command value)
        {
            var result = await mediator.Send(value);

            return null;
        }
    }
}
