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
            List<DateTime> l = new List<DateTime>()
            {
                new DateTime(2019,01,01,17,30,00),
                new DateTime(2018,02,20,00,00,00),
                new DateTime(2018,01,15,12,00,00),
                new DateTime(2018,04,15,7,00,00),
                new DateTime(2018,05,15,16,00,00),
                new DateTime(2018,11,15,17,29,59),
                new DateTime(2018,11,07,12,00,00),
                new DateTime(2018,08,15,17,30,00),
                new DateTime(2018,09,15,18,00,00),
                new DateTime(2018,10,15,12,00,00),
                new DateTime(2018,12,20,12,00,00),
                new DateTime(2018,12,25,12,12,01),
            };

            foreach (var item in l)
            {
                CalculateMethods.VerifyTariffType(item);
            }
            return null;
        }

        [HttpPost]
        public async Task<Calculate.ResponseModel> Calculate([FromBody] Calculate.RequestModel req)
        {
            return null;
        }

    }
}
