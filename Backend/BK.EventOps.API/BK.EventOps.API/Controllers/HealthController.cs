using Microsoft.AspNetCore.Mvc;

namespace BK.EventOps.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = System.DateTime.UtcNow,
                service = "BK.EventOps.API",
                version = "1.0.0"
            });
        }
    }
}
