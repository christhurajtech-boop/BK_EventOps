using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BK.EventOps.Application.Interface;
using BK.EventOps.Common.Models;
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
            if (model == null)
            {
                return BadRequest(new { error = "Attendance payload is required." });
            }

            if (string.IsNullOrWhiteSpace(model.EmployeeId) || string.IsNullOrWhiteSpace(model.Date))
            {
                return BadRequest(new { error = "EmployeeId and Date are required." });
            }

            var saved = await _service.SaveAttendanceAsync(model, ct);
            return Ok(saved);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<AttendanceModel>>> MarkBulk([FromBody] List<AttendanceModel> models, CancellationToken ct)
        {
            if (models == null || models.Count == 0)
            {
                return BadRequest(new { error = "Attendance batch cannot be empty." });
            }

            // Atomic validation: any invalid item rejects the entire batch
            foreach (var item in models)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.EmployeeId) || string.IsNullOrWhiteSpace(item.Date))
                {
                    return BadRequest(new { error = "Invalid attendance item in batch: EmployeeId and Date are required." });
                }
            }

            var saved = await _service.SaveBulkAttendanceAsync(models, ct);
            return Ok(saved);
        }
    }
}
