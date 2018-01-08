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
            return null;
        }

        [HttpPost]
        public async Task<Calculate.ResponseModel> Calculate([FromBody] Calculate.RequestModel req)
        {
            return null;
        }

    }
}
