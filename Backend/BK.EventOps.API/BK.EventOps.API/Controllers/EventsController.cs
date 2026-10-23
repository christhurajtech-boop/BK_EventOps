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
    public class EventsController : ControllerBase
    {
        private readonly IEventOpsService _service;

        public EventsController(IEventOpsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventModel>>> GetAll(CancellationToken ct)
        {
            var result = await _service.GetEventsAsync(ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventModel>> GetById(string id, CancellationToken ct)
        {
            var ev = await _service.GetEventByIdAsync(id, ct);
            if (ev == null) return NotFound(new { message = $"Event with id {id} not found." });
            return Ok(ev);
        }

        [HttpPost]
        public async Task<ActionResult<EventModel>> Save([FromBody] EventModel model, CancellationToken ct)
        {
            var saved = await _service.SaveEventAsync(model, ct);
            return Ok(saved);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EventModel>> Update(string id, [FromBody] EventModel model, CancellationToken ct)
        {
            model.EventId = id;
            var updated = await _service.SaveEventAsync(model, ct);
            return Ok(updated);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<EventModel>> UpdateStatus(string id, [FromBody] UpdateEventStatusRequest request, CancellationToken ct)
        {
            var updated = await _service.UpdateEventStatusAsync(id, request.Status, ct);
            if (updated == null) return NotFound(new { message = $"Event with id {id} not found." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var deleted = await _service.DeleteEventAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
