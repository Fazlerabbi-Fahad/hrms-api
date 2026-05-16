using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.PaymentStatus;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PaymentStatusController : BaseController
    {
        public readonly IPaymentStatusService _PaymentStatusService;

        public PaymentStatusController(IPaymentStatusService PaymentStatusService)
        {
            _PaymentStatusService = PaymentStatusService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllPaymentStatuss([FromQuery] QueryParameters parameters)
        {
            var result = await _PaymentStatusService.GetAllPaymentStatussAsync(parameters);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentStatusById(int id)
        {
            var result = await _PaymentStatusService.GetPaymentStatusByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> CreatePaymentStatus([FromBody] PaymentStatusRequestDto dto)
        {
            var result = await _PaymentStatusService.CreatePaymentStatusAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> UpdatePaymentStatus(int id, [FromBody] PaymentStatusUpdateRequestDto dto)
        {
            var result = await _PaymentStatusService.UpdatePaymentStatusAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> DeletePaymentStatus(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _PaymentStatusService.DeletePaymentStatusAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
