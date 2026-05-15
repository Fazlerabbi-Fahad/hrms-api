using HRMS.Application.DTOs.Employee;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IEmployeeRepository
    {
        Task<(List<Employee>,int totalCount)> GetAllEmployeesAsync(EmployeeQueryParameters parameters);
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> DeleteEmployeeAsync(int id,int userId);
    }
}
