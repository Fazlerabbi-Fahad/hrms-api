using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Designation;

namespace HRMS.Application.Interfaces
{
    public interface IDesignationService
    {
        Task<ApiResponse<PagedResult<DesignationResponseDto>>> GetAllDesignationsAsync(QueryParameters parameters);
        Task<ApiResponse<DesignationResponseDto>> GetDesignationByIdAsync(int id);
        Task<ApiResponse<DesignationResponseDto>> CreateDesignationAsync(DesignationRequestDto dto);
        Task<ApiResponse<DesignationResponseDto>> UpdateDesignationAsync(int id, DesignationUpdateRequestDto dto);
        Task<ApiResponse<bool>> DeleteDesignationAsync(int id, int userId);
    }
}
