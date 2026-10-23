using System;
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

        private static readonly HashSet<string> ValidStatuses = new(StringComparer.OrdinalIgnoreCase)
        {
            "enquiry", "confirmed", "completed", "cancelled"
        };

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
            if (model == null)
            {
                return BadRequest(new { error = "Event payload is required." });
            }

            if (model.QuotedAmount < 0)
            {
                return BadRequest(new { error = "Quoted budget amount cannot be negative." });
            }

            if (!string.IsNullOrWhiteSpace(model.EndDate) && !string.IsNullOrWhiteSpace(model.EventDate))
            {
                if (DateTime.TryParse(model.EndDate, out var end) && DateTime.TryParse(model.EventDate, out var start))
                {
                    if (end < start)
                    {
                        return BadRequest(new { error = "Event end date cannot be earlier than start date." });
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(model.Status) && !ValidStatuses.Contains(model.Status))
            {
                return BadRequest(new { error = "Invalid event status value." });
            }

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
            if (request == null || string.IsNullOrWhiteSpace(request.Status))
            {
                return BadRequest(new { error = "Status is required." });
            }

            if (!ValidStatuses.Contains(request.Status))
            {
                return BadRequest(new { error = "Status must be enquiry, confirmed, completed, or cancelled." });
            }

            var updated = await _service.UpdateEventStatusAsync(id, request.Status.ToLowerInvariant(), ct);
            if (updated == null) return NotFound(new { message = $"Event with id {id} not found." });
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var ev = await _service.GetEventByIdAsync(id, ct);
            if (ev == null) return NotFound();

            var hasTransactions = await _service.HasTransactionsForEventAsync(id, ct);
            if (hasTransactions)
            {
                return Conflict(new { error = "Cannot delete event with existing financial transactions." });
            }

            var deleted = await _service.DeleteEventAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
