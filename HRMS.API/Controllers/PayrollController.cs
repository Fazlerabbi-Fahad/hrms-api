using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Payroll;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PayrollController : BaseController
    {
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollService payrollService)
        {
            _payrollService = payrollService;
        }

        [HttpGet]
        [Authorize(Roles = $"{AppConstants.Roles.Admin}," +
                           $"{AppConstants.Roles.HRAdmin},")]
        public async Task<IActionResult> GetAll(
            [FromQuery] PayrollQueryParameters parameters)
        {
            var response = await _payrollService.GetAllAsync(parameters);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{AppConstants.Roles.Admin}," +
                           $"{AppConstants.Roles.HRAdmin}," )]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _payrollService.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("process")]
        [Authorize(Roles = $"{AppConstants.Roles.Admin},")]
        public async Task<IActionResult> ProcessPayroll(
            [FromBody] PayrollRequestDto dto)
        {
            dto.UserId = GetCurrentUserId();
            var response = await _payrollService.ProcessPayrollAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}/mark-paid")]
        [Authorize(Roles = $"{AppConstants.Roles.Admin}," )]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var userId = GetCurrentUserId();
            var response = await _payrollService.MarkAsPaidAsync(id, userId);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("report")]
        [Authorize(Roles = $"{AppConstants.Roles.Admin}," +
                           $"{AppConstants.Roles.HRAdmin}," )]
        public async Task<IActionResult> GetMonthlyReport(
            [FromQuery] int month,
            [FromQuery] int year)
        {
            var response = await _payrollService
                .GetMonthlyReportAsync(month, year);
            return StatusCode(response.StatusCode, response);
        }
    }
}
