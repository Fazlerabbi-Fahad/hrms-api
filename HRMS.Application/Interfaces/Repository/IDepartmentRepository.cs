using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Department;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IDepartmentRepository
    {
        Task<(List<Department>, int totalCount)> GetAllDepartmentsAsync(QueryParameters parameters);
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<Department> CreateDepartmentAsync(Department Department);
        Task<Department> UpdateDepartmentAsync(int id, Department Department);
        Task<bool> DeleteDepartmentAsync(int id, int userId);
    }
}
