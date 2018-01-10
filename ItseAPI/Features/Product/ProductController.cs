using ItseAPI.Features;
using ItseAPI.Features.Calculate;
using ItseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ItseAPI.Features.Product
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterProduct([FromBody] RegisterProduct.Command value)
        {
            var result = await mediator.Send(value);

            return Created(this.Request.Path.Value + "/" + result.Id, result);
        }

        [HttpGet]
        public async Task<List<SearchManyProducts.Result>> SearchManyProduct([FromQuery] SearchManyProducts.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<SearchOneProduct.Result> SearchOneProduct([FromRoute] SearchOneProduct.Query query)
        {
            var result = await mediator.Send(query);

            return result;
        }
    }
}
