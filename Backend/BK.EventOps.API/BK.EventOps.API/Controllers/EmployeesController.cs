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
    public class EmployeesController : ControllerBase
    {
        private readonly IEventOpsService _service;

        public EmployeesController(IEventOpsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetAll(CancellationToken ct)
        {
            var result = await _service.GetEmployeesAsync(ct);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeModel>> GetById(string id, CancellationToken ct)
        {
            var emp = await _service.GetEmployeeByIdAsync(id, ct);
            if (emp == null) return NotFound(new { message = $"Employee with id {id} not found." });
            return Ok(emp);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeModel>> Save([FromBody] EmployeeModel model, CancellationToken ct)
        {
            var saved = await _service.SaveEmployeeAsync(model, ct);
            return Ok(saved);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeModel>> Update(string id, [FromBody] EmployeeModel model, CancellationToken ct)
        {
            model.EmployeeId = id;
            var updated = await _service.SaveEmployeeAsync(model, ct);
            return Ok(updated);
        }

        [HttpPatch("{id}/toggle-status")]
        public async Task<ActionResult<object>> ToggleStatus(string id, CancellationToken ct)
        {
            var updated = await _service.ToggleEmployeeStatusAsync(id, ct);
            if (updated == null) return NotFound(new { message = $"Employee with id {id} not found." });
            return Ok(new { employeeId = updated.EmployeeId, status = updated.Status });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ct)
        {
            var deleted = await _service.DeleteEmployeeAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
