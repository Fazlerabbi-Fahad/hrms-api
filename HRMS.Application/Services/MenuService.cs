using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Menu;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly ILogger<MenuService> _logger;

        public MenuService(
            IMenuRepository menuRepository,
            ILogger<MenuService> logger)
        {
            _menuRepository = menuRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<List<MenuResponseDto>>> GetUserWiseMenuAsync(
            int userId)
        {
            var menus = await _menuRepository.GetUserWiseMenuAsync(userId);

            if (!menus.Any())
            {
                _logger.LogWarning(
                    "No menu items found for user {UserId}", userId);

                return ApiResponse<List<MenuResponseDto>>.Success(
                    new List<MenuResponseDto>(),
                    "No menu items assigned to this user");
            }

            _logger.LogInformation(
                "Retrieved {Count} menu items for user {UserId}",
                menus.Count, userId);

            return ApiResponse<List<MenuResponseDto>>.Success(
                menus,
                $"Retrieved {menus.Count} menu items");
        }
    }
}
