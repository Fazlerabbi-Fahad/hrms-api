using HRMS.Application.DTOs.Employee;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(EmployeeRequestDto employee);
        Task<Employee> UpdateEmployeeAsync(int id,EmployeeUpdateRequestDto employee);
        Task<bool> DeleteEmployeeAsync(int id,int userId);
    }
}
