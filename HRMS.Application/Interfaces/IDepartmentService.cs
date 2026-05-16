using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Department;

namespace HRMS.Application.Interfaces
{
    public interface IDepartmentService
    {
        Task<ApiResponse<PagedResult<DepartmentResponseDto>>> GetAllDepartmentsAsync(QueryParameters parameters);
        Task<ApiResponse<DepartmentResponseDto>> GetDepartmentByIdAsync(int id);
        Task<ApiResponse<DepartmentResponseDto>> CreateDepartmentAsync(DepartmentRequestDto dto);
        Task<ApiResponse<DepartmentResponseDto>> UpdateDepartmentAsync(int id, DepartmentUpdateRequestDto dto);
        Task<ApiResponse<bool>> DeleteDepartmentAsync(int id, int userId);
    }
}
