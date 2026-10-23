using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BK.EventOps.Application.Interface;
using BK.EventOps.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BK.EventOps.API.Controllers
{
    [ApiController]
    [Route("api/salary-payments")]
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

        [HttpPost]
        public async Task<ActionResult<SalaryPaymentModel>> ProcessPayout([FromBody] CreateSalaryPayoutRequest request, CancellationToken ct)
        {
            var result = await _service.ProcessSalaryPayoutAsync(request, ct);
            return Ok(result);
        }
    }
}
