using Microsoft.AspNetCore.Mvc;

namespace BK.EventOps.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IEventOpsService _service;

        public AttendanceController(IEventOpsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceModel>>> GetAll(CancellationToken ct)
        {
            var result = await _service.GetAttendanceAsync(ct);
            return Ok(result);
        }

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<AttendanceModel>>> GetByEvent(string eventId, CancellationToken ct)
        {
            var result = await _service.GetAttendanceByEventAsync(eventId, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AttendanceModel>> Mark([FromBody] AttendanceModel model, CancellationToken ct)
        {
            var saved = await _service.SaveAttendanceAsync(model, ct);
            return Ok(saved);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<AttendanceModel>>> MarkBulk([FromBody] List<AttendanceModel> models, CancellationToken ct)
        {
            var saved = await _service.SaveBulkAttendanceAsync(models, ct);
            return Ok(saved);
        }
    }
}
