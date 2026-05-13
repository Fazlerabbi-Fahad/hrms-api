using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;

        public EmployeeRepository(HRMSDbContext hrmsDbContext)
        {
            _hrmsDbContext = hrmsDbContext;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _hrmsDbContext.Employees
                        .Include(e => e.Department)
                        .Include(e => e.Designation)
                        .Include(e => e.EmploymentStatus)
                        .Where(e => e.IsActive)
                        .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(int id)
        {
            return await _hrmsDbContext.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .Include(e => e.EmploymentStatus)
                .Where(e => e.Id == id && e.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new InvalidOperationException("Employee data is null");
            }
            var existingEmployee = await _hrmsDbContext.Employees.Where(x => (x.PhoneNumber == employee.PhoneNumber || x.Email == employee.Email) && x.IsActive).FirstOrDefaultAsync();
            if (existingEmployee != null)
            {
                throw new InvalidOperationException("Employee with this email or phone already exists");
            }
            var previousEmployee = await _hrmsDbContext.Employees.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var empCode = await GenerateEmployeeCode(previousEmployee?.EmpCode);

            employee.EmpCode = empCode;
            employee.IsActive = true;
            employee.CreatedAt = DateTime.UtcNow;

            _hrmsDbContext.Employees.Add(employee);
            await _hrmsDbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = await _hrmsDbContext.Employees.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (existingEmployee == null)
            {
                throw new Exception("Employee not found");
            }

            existingEmployee.UpdatedAt = DateTime.UtcNow;

            await _hrmsDbContext.SaveChangesAsync();
            return existingEmployee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id, int userId)
        {
            var employee = await _hrmsDbContext.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (employee == null)
            {
                return false;
            }
            employee.IsActive = false;
            employee.UpdatedAt = DateTime.UtcNow;
            employee.UpdatedBy = userId;

            await _hrmsDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _hrmsDbContext.Employees
                        .Where(e => e.Id == id && e.IsActive)
                        .AnyAsync();
        }

        private async Task<string> GenerateEmployeeCode(string? code)
        {
            int nextNumber = 1;

            if (!string.IsNullOrEmpty(code) && code.StartsWith("EMP"))
            {
                string numberPart = code.Replace("EMP", "");
                if (int.TryParse(numberPart, out int currentNumber))
                {
                    nextNumber = currentNumber + 1;
                }
            }
            return $"EMP{nextNumber:D4}";
        }
    }
}