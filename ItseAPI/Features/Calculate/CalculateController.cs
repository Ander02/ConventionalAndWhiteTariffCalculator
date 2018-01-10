using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItseAPI.Features.Calculate
{
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
            var c = new Models.Calculate.DateInitAndFinish()
            {
                DateTimeInit = new DateTime(2018, 01, 01, 00, 00, 00),
                DateTimeFinish = new DateTime(2018, 01, 01, 23, 20, 00)
            };

            var e = TariffAuxMethods.SamePeriod(c.DateTimeInit, c.DateTimeFinish);


            var result = await mediator.Send(value);

            return null;
        }
    }
}
