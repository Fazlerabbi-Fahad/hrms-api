using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Payroll;

namespace HRMS.Application.Interfaces
{
    public interface IPayrollService
    {
        Task<ApiResponse<PagedResult<PayrollResponseDto>>> GetAllAsync(
            PayrollQueryParameters parameters);
        Task<ApiResponse<PayrollResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<PayrollResponseDto>> ProcessPayrollAsync(
            PayrollRequestDto dto);
        Task<ApiResponse<PayrollResponseDto>> MarkAsPaidAsync(
            int id, int userId);
        Task<ApiResponse<PayrollReportDto>> GetMonthlyReportAsync(
            int month, int year);
    }
}
