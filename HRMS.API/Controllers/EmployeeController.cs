using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class EmployeeController : BaseController
    {
        public readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees([FromQuery] EmployeeQueryParameters parameters)
        {
            var result = await _employeeService.GetAllEmployeesAsync(parameters);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var result = await _employeeService.GetEmployeeByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRequestDto dto)
        {
            var result = await _employeeService.CreateEmployeeAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdateRequestDto dto)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _employeeService.DeleteEmployeeAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}