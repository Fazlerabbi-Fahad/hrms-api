using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Department;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class DepartmentController : BaseController
    {
        public readonly IDepartmentService _DepartmentService;

        public DepartmentController(IDepartmentService DepartmentService)
        {
            _DepartmentService = DepartmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments([FromQuery] QueryParameters parameters)
        {
            var result = await _DepartmentService.GetAllDepartmentsAsync(parameters);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var result = await _DepartmentService.GetDepartmentByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentRequestDto dto)
        {
            var result = await _DepartmentService.CreateDepartmentAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpdateRequestDto dto)
        {
            var result = await _DepartmentService.UpdateDepartmentAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _DepartmentService.DeleteDepartmentAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
