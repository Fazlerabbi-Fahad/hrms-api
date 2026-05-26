using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Menu;

namespace HRMS.Application.Interfaces
{
    public interface IMenuService
    {
        Task<ApiResponse<List<MenuResponseDto>>> GetUserWiseMenuAsync(int userId);
    }
}
