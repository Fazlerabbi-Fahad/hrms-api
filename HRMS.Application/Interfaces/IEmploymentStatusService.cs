using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.EmploymentStatus;

namespace HRMS.Application.Interfaces
{
    public interface IEmploymentStatusService
    {
        Task<ApiResponse<PagedResult<EmploymentStatusResponseDto>>> GetAllEmploymentStatussAsync(QueryParameters parameters);
        Task<ApiResponse<EmploymentStatusResponseDto>> GetEmploymentStatusByIdAsync(int id);
        Task<ApiResponse<EmploymentStatusResponseDto>> CreateEmploymentStatusAsync(EmploymentStatusRequestDto dto);
        Task<ApiResponse<EmploymentStatusResponseDto>> UpdateEmploymentStatusAsync(int id, EmploymentStatusUpdateRequestDto dto);
        Task<ApiResponse<bool>> DeleteEmploymentStatusAsync(int id, int userId);
    }
}
