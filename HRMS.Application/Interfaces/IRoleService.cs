using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Role;

namespace HRMS.Application.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResponse<PagedResult<RoleResponseDto>>> GetAllRolesAsync(QueryParameters parameters);
        Task<ApiResponse<RoleResponseDto>> GetRoleByIdAsync(int id);
        Task<ApiResponse<RoleResponseDto>> CreateRoleAsync(RoleRequestDto dto);
        Task<ApiResponse<RoleResponseDto>> UpdateRoleAsync(int id, RoleUpdateRequestDto dto);
        Task<ApiResponse<bool>> DeleteRoleAsync(int id, int userId);
    }
}
