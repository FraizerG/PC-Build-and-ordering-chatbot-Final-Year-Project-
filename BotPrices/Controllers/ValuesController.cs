using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace BotPrices.Controllers
{
    [Route("api/[controller]")] //API web controller
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ObjectResult GetPrices () // method to get the prices
        {
            return Ok(System.IO.File.ReadAllText("results.json")); //read JSON File
        }
    }
}
