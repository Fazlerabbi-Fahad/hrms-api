using HRMS.Application.DTOs.Payroll;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IPayrollRepository
    {
        Task<(List<PayrollResponseDto> payrolls, int totalCount)> GetAllAsync(
            PayrollQueryParameters parameters);
        Task<PayrollResponseDto?> GetByIdAsync(int id);
        Task<PayrollReportDto?> GetMonthlyReportAsync(int month, int year);
        Task<bool> ExistsAsync(int employeeId, int month, int year);
        Task<Payroll> CreateAsync(Payroll payroll);
        Task<Payroll?> GetEntityByIdAsync(int id);
        Task<Payroll> UpdateAsync(Payroll payroll);
    }
}
