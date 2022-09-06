using AWPurchaseCore5API.Models.Output;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWPurchaseCore5API.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : Controller
    {
        [Route("check")]
        [HttpGet]
        public IActionResult check()
        {
            var response = new StatusResponse
            {
                Success = true
            };
            return Ok(response);
           
        }
    }
}
