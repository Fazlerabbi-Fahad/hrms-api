using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Salary;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class SalaryController : BaseController
    {
        public readonly ISalaryService _SalaryService;

        public SalaryController(ISalaryService SalaryService)
        {
            _SalaryService = SalaryService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllSalarys([FromQuery] SalaryQueryParameters parameters)
        {
            var result = await _SalaryService.GetAllSalarysAsync(parameters);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetSalaryById(int id)
        {
            var result = await _SalaryService.GetSalaryByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> CreateSalary([FromBody] SalaryRequestDto dto)
        {
            var result = await _SalaryService.CreateSalaryAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> UpdateSalary(int id, [FromBody] SalaryUpdateRequestDto dto)
        {
            var result = await _SalaryService.UpdateSalaryAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> DeleteSalary(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _SalaryService.DeleteSalaryAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
