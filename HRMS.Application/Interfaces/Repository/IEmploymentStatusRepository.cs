using HRMS.Application.DTOs.Common;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IEmploymentStatusRepository
    {
        Task<(List<EmploymentStatus>, int totalCount)> GetAllEmploymentStatusAsync(QueryParameters parameters);
        Task<EmploymentStatus?> GetEmploymentStatusByIdAsync(int id);
        Task<EmploymentStatus> CreateEmploymentStatusAsync(EmploymentStatus EmploymentStatus);
        Task<EmploymentStatus> UpdateEmploymentStatusAsync(int id, EmploymentStatus EmploymentStatus);
        Task<bool> DeleteEmploymentStatusAsync(int id, int userId);
    }
}
