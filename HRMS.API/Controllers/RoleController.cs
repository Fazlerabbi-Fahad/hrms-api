using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Role;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class RoleController : BaseController
    {
        public readonly IRoleService _RoleService;

        public RoleController(IRoleService RoleService)
        {
            _RoleService = RoleService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllRoles([FromQuery] QueryParameters parameters)
        {
            var result = await _RoleService.GetAllRolesAsync(parameters);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var result = await _RoleService.GetRoleByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> CreateRole([FromBody] RoleRequestDto dto)
        {
            var result = await _RoleService.CreateRoleAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleUpdateRequestDto dto)
        {
            var result = await _RoleService.UpdateRoleAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _RoleService.DeleteRoleAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
