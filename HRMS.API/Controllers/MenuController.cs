using Asp.Versioning;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("user-menu")]
        public async Task<IActionResult> GetUserMenu()
        {
            var userId = GetCurrentUserId();
            var response = await _menuService.GetUserWiseMenuAsync(userId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
