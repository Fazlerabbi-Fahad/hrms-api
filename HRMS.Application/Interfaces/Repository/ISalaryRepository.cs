using HRMS.Application.DTOs.Salary;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface ISalaryRepository
    {
        Task<(List<Salary>, int totalCount)> GetAllSalaryAsync(SalaryQueryParameters parameters);
        Task<Salary?> GetSalaryByIdAsync(int id);
        Task<Salary> CreateSalaryAsync(Salary Salary);
        Task<Salary> UpdateSalaryAsync(int id, Salary Salary);
        Task<bool> DeleteSalaryAsync(int id, int userId);
    }
}
