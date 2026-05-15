using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Salary;

namespace HRMS.Application.Interfaces
{
    public interface ISalaryService
    {
        Task<ApiResponse<PagedResult<SalaryResponseDto>>> GetAllSalarysAsync(SalaryQueryParameters parameters);
        Task<ApiResponse<SalaryResponseDto>> GetSalaryByIdAsync(int id);
        Task<ApiResponse<SalaryResponseDto>> CreateSalaryAsync(SalaryRequestDto dto);
        Task<ApiResponse<SalaryResponseDto>> UpdateSalaryAsync(int id, SalaryUpdateRequestDto dto);
        Task<ApiResponse<bool>> DeleteSalaryAsync(int id, int userId);
    }
}
