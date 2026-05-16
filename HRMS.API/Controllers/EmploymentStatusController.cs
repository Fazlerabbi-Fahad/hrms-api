using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.EmploymentStatus;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class EmploymentStatusController : BaseController
    {
        public readonly IEmploymentStatusService _EmploymentStatusService;

        public EmploymentStatusController(IEmploymentStatusService EmploymentStatusService)
        {
            _EmploymentStatusService = EmploymentStatusService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllEmploymentStatuss([FromQuery] QueryParameters parameters)
        {
            var result = await _EmploymentStatusService.GetAllEmploymentStatussAsync(parameters);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmploymentStatusById(int id)
        {
            var result = await _EmploymentStatusService.GetEmploymentStatusByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> CreateEmploymentStatus([FromBody] EmploymentStatusRequestDto dto)
        {
            var result = await _EmploymentStatusService.CreateEmploymentStatusAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> UpdateEmploymentStatus(int id, [FromBody] EmploymentStatusUpdateRequestDto dto)
        {
            var result = await _EmploymentStatusService.UpdateEmploymentStatusAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> DeleteEmploymentStatus(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _EmploymentStatusService.DeleteEmploymentStatusAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
