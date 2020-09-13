using Microsoft.AspNetCore.Mvc;

namespace EnpalSharpTemplate.Controllers.V1_0
{
    [RequireHttps]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HealthController : Controller
    {
        // GET: api/<HealthController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("alive");
        }
    }
}
