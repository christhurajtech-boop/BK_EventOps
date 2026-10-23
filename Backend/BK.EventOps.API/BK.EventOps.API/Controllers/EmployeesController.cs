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
            if (model == null)
            {
                return BadRequest(new { error = "Employee payload is required." });
            }

            var fieldErrors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                fieldErrors["name"] = new[] { "Name is required." };
            }

            var normalizedSalaryType = (model.SalaryType ?? "").ToLowerInvariant();
            if (normalizedSalaryType != "daily" && normalizedSalaryType != "monthly")
            {
                fieldErrors["salary_type"] = new[] { "Invalid salary_type. Must be 'daily' or 'monthly'." };
            }

            if (fieldErrors.Count > 0)
            {
                return BadRequest(new { error = "Validation failed.", errors = fieldErrors });
            }

            // Negative rates check
            if (model.RateOnSite < 0 || model.RateOffSite < 0 || model.MonthlySalary < 0)
            {
                return BadRequest(new { error = "Salary rates and amounts cannot be negative." });
            }

            // Monthly salary must be > 0 for monthly staff
            if (normalizedSalaryType == "monthly" && model.MonthlySalary <= 0)
            {
                return BadRequest(new { error = "Monthly salary must be greater than zero for monthly employees." });
            }

            model.SalaryType = normalizedSalaryType;
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

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<EmployeeModel>> UpdateStatus(string id, [FromBody] UpdateEmployeeStatusRequest request, CancellationToken ct)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Status))
            {
                return BadRequest(new { error = "Status is required." });
            }

            var status = request.Status.ToLowerInvariant();
            if (status != "active" && status != "inactive")
            {
                return BadRequest(new { error = "Invalid status value. Must be 'active' or 'inactive'." });
            }

            var updated = await _service.UpdateEmployeeStatusAsync(id, status, ct);
            if (updated == null) return NotFound(new { message = $"Employee with id {id} not found." });
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
            var emp = await _service.GetEmployeeByIdAsync(id, ct);
            if (emp == null) return NotFound();

            var hasAttendance = await _service.HasAttendanceForEmployeeAsync(id, ct);
            if (hasAttendance)
            {
                return Conflict(new { error = "Cannot delete employee with existing attendance records." });
            }

            var deleted = await _service.DeleteEmployeeAsync(id, ct);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
