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
    public class TransactionsController : ControllerBase
    {
        private readonly IEventOpsService _service;

        public TransactionsController(IEventOpsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventTransactionModel>>> GetAll(CancellationToken ct)
        {
            var result = await _service.GetTransactionsAsync(ct);
            return Ok(result);
        }

        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EventTransactionModel>>> GetByEvent(string eventId, CancellationToken ct)
        {
            var result = await _service.GetTransactionsByEventAsync(eventId, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<EventTransactionModel>> Save([FromBody] EventTransactionModel model, CancellationToken ct)
        {
            var saved = await _service.SaveTransactionAsync(model, ct);
            return Ok(saved);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var deleted = await _service.DeleteTransactionAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
