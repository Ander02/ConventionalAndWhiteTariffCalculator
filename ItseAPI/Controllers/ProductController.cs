using ItseAPI.Features;
using ItseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTestAPI.Models;

namespace ItseAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {

        [HttpGet]
        public async Task<ProductResponseModel> ProductByName([FromQuery] string name)
        {
            var c = new Models.Calculate.DateInitAndFinish()
            {
                DateTimeInit = new DateTime(2018, 01, 01, 00, 00, 00),
                DateTimeFinish = new DateTime(2018, 01, 01, 23, 20, 00)
            };

            var e = CalculateMethods.SamePeriod(c.DateTimeInit, c.DateTimeFinish);

            return null;
        }

        [HttpPost]
        public async Task<Calculate.ResponseModel> Calculate([FromBody] Calculate.RequestModel req)
        {
            return null;
        }

    }
}
