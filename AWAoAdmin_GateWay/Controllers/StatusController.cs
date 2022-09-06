using AWAoAdmin_GateWay.Model.Output;
using Microsoft.AspNetCore.Mvc;

namespace AWAoAdmin_GateWay.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : Controller
    {
        [HttpGet("check")]
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
