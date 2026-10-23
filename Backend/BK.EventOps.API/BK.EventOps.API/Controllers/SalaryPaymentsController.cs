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
    [Route("api/salary-payments")]
    [Route("api/salarypayments")]
    public class SalaryPaymentsController : ControllerBase
    {
        private readonly IEventOpsService _service;

        public SalaryPaymentsController(IEventOpsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalaryPaymentModel>>> GetAll(CancellationToken ct)
        {
            var result = await _service.GetSalaryPaymentsAsync(ct);
            return Ok(result);
        }

        [HttpGet("employee/{empId}")]
        public async Task<ActionResult<IEnumerable<SalaryPaymentModel>>> GetByEmployee(string empId, CancellationToken ct)
        {
            var result = await _service.GetSalaryPaymentsByEmployeeAsync(empId, ct);
            return Ok(result ?? new List<SalaryPaymentModel>());
        }

        [HttpPost]
        [HttpPost("payout")]
        public async Task<ActionResult<SalaryPaymentModel>> ProcessPayout([FromBody] CreateSalaryPayoutRequest request, CancellationToken ct)
        {
            if (request == null)
            {
                return BadRequest(new { error = "Payout payload is required." });
            }

            if (string.IsNullOrWhiteSpace(request.EmployeeId))
            {
                return BadRequest(new { error = "EmployeeId is required." });
            }

            var emp = await _service.GetEmployeeByIdAsync(request.EmployeeId, ct);
            if (emp == null)
            {
                return NotFound(new { error = $"Employee with id {request.EmployeeId} not found." });
            }

            var isMonthly = string.Equals(request.SalaryType, "monthly", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(emp.SalaryType, "monthly", StringComparison.OrdinalIgnoreCase);

            var existingPayouts = await _service.GetSalaryPaymentsByEmployeeAsync(request.EmployeeId, ct);

            if (isMonthly)
            {
                var targetMonth = string.IsNullOrWhiteSpace(request.Month) ? DateTime.UtcNow.ToString("yyyy-MM") : request.Month;

                // Check for duplicate payout for same month
                if (existingPayouts.Any(p => p.Month == targetMonth))
                {
                    return Conflict(new { error = $"Staff has already received monthly salary for {targetMonth}." });
                }

                // Verify client amount against server-computed monthly wage
                var serverMonthlyWage = emp.MonthlySalary;
                if (serverMonthlyWage > 0 && Math.Abs(request.AmountPaid - serverMonthlyWage) > 0.01m)
                {
                    return BadRequest(new { error = $"Client amount ({request.AmountPaid}) does not match server-computed wages ({serverMonthlyWage})." });
                }
            }
            else
            {
                // Daily wage payout
                if (request.AttendanceIds == null || request.AttendanceIds.Count == 0)
                {
                    return BadRequest(new { error = "Please select at least 1 work day to pay." });
                }

                var allAttendance = await _service.GetAttendanceAsync(ct);
                var attMap = allAttendance.ToDictionary(a => a.AttendanceId);

                // Check for already paid attendances
                foreach (var attId in request.AttendanceIds)
                {
                    if (attMap.TryGetValue(attId, out var attItem))
                    {
                        if (string.Equals(attItem.PaymentStatus, "paid", StringComparison.OrdinalIgnoreCase))
                        {
                            return Conflict(new { error = $"Attendance ID '{attId}' has already been paid." });
                        }
                    }
                }

                // Verify client amount against server-computed wage sum
                decimal serverWageSum = 0;
                foreach (var attId in request.AttendanceIds)
                {
                    if (attMap.TryGetValue(attId, out var attItem))
                    {
                        serverWageSum += attItem.RateApplied;
                    }
                }

                if (Math.Abs(request.AmountPaid - serverWageSum) > 0.01m)
                {
                    return BadRequest(new { error = $"Client amount ({request.AmountPaid}) does not match server-computed wages ({serverWageSum})." });
                }
            }

            var result = await _service.ProcessSalaryPayoutAsync(request, ct);
            return Ok(result);
        }
    }
}
