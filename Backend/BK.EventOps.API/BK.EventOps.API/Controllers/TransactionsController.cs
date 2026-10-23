using System;
using System.Collections.Generic;
using System.Linq;
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
            if (model == null)
            {
                return BadRequest(new { error = "Transaction payload is required." });
            }

            // Amount 0 or negative
            if (model.Amount <= 0)
            {
                return BadRequest(new { error = "Transaction amount must be greater than zero." });
            }

            // More than 2 decimal places check
            if (decimal.Round(model.Amount, 2) != model.Amount)
            {
                return BadRequest(new { error = "Amount cannot have more than 2 decimal places." });
            }

            // If income payment linked to an event, check if payment exceeds remaining due
            if (model.Type == "income" && !string.IsNullOrWhiteSpace(model.EventId))
            {
                var ev = await _service.GetEventByIdAsync(model.EventId, ct);
                if (ev != null && ev.QuotedAmount > 0)
                {
                    var existingTransactions = await _service.GetTransactionsByEventAsync(model.EventId, ct);
                    var currentCollected = existingTransactions.Where(t => t.Type == "income" && t.TransactionId != model.TransactionId).Sum(t => t.Amount);
                    var remainingDue = ev.QuotedAmount - currentCollected;

                    if (model.Amount > remainingDue + 0.001m)
                    {
                        return BadRequest(new { error = $"Payment amount ({model.Amount}) exceeds remaining balance of {remainingDue}." });
                    }
                }
            }

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
