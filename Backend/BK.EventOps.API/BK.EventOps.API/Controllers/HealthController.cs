using System;
using Microsoft.AspNetCore.Mvc;

namespace BK.EventOps.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private static readonly DateTime StartTime = DateTime.UtcNow;

        [HttpGet]
        public IActionResult Get()
        {
            var uptime = DateTime.UtcNow - StartTime;
            return Ok(new
            {
                status = "healthy",
                uptime = uptime.ToString(),
                uptimeSeconds = (long)uptime.TotalSeconds,
                timestamp = DateTime.UtcNow,
                service = "BK.EventOps.API",
                version = "1.0.0"
            });
        }
    }
}
