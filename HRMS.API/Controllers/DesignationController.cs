using Asp.Versioning;
using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Designation;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class DesignationController : BaseController
    {
        public readonly IDesignationService _DesignationService;

        public DesignationController(IDesignationService DesignationService)
        {
            _DesignationService = DesignationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDesignations([FromQuery] QueryParameters parameters)
        {
            var result = await _DesignationService.GetAllDesignationsAsync(parameters);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetDesignationById(int id)
        {
            var result = await _DesignationService.GetDesignationByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> CreateDesignation([FromBody] DesignationRequestDto dto)
        {
            var result = await _DesignationService.CreateDesignationAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin + "," + AppConstants.Roles.HRAdmin)]
        public async Task<IActionResult> UpdateDesignation(int id, [FromBody] DesignationUpdateRequestDto dto)
        {
            var result = await _DesignationService.UpdateDesignationAsync(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppConstants.Roles.Admin)]
        public async Task<IActionResult> DeleteDesignation(int id)
        {
            var userId = GetCurrentUserId();
            var result = await _DesignationService.DeleteDesignationAsync(id, userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
