using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(HRMSDbContext hrmsDbContext, ILogger<EmployeeRepository> logger)
        {
            _hrmsDbContext = hrmsDbContext;
            _logger = logger;
        }

        public async Task<(List<Employee>,int totalCount)> GetAllEmployeesAsync(EmployeeQueryParameters parameters)
        {
                var query = _hrmsDbContext.Employees
                        .Include(e => e.Department)
                        .Include(e => e.Designation)
                        .Include(e => e.EmploymentStatus)
                        .Where(e => e.IsActive);

                if (parameters.DesignationId.HasValue)
                {
                    query = query.Where(e => e.DesignationId == parameters.DesignationId.Value);
                }

                if (parameters.DepartmentId.HasValue)
                {
                    query = query.Where(e => e.DepartmentId == parameters.DepartmentId.Value);
                }

                if (parameters.EmploymentStatusId.HasValue)
                {
                    query = query.Where(e => e.EmploymentStatusId == parameters.EmploymentStatusId.Value);
                }

                if (!string.IsNullOrEmpty(parameters.Search))
                    query = query.Where(e =>
                        e.Name.Contains(parameters.Search) ||
                        e.Email!.Contains(parameters.Search) ||
                        e.PhoneNumber.Contains(parameters.Search));

                query = parameters.SortBy?.ToLower() switch
                {
                    "name" => parameters.SortDirection == "desc"
                            ? query.OrderByDescending(e => e.Name)
                            : query.OrderBy(e => e.Name),
                    "joiningdate" => parameters.SortDirection == "desc"
                            ? query.OrderByDescending(e => e.JoiningDate)
                            : query.OrderBy(e => e.JoiningDate),
                    _ => query.OrderBy(e => e.Id),
                };

                var totalCount = await query.CountAsync();

                var employee = await query
                                    .Skip((parameters.PageNumber - 1) * totalCount)
                                    .Take(parameters.PageSize)
                                    .ToListAsync();
                return (employee, totalCount);
         
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
                _logger.LogWarning("Employee already found!", employee.Name);
                throw new InvalidOperationException("Employee with this email or phone already exists");
            }
            var previousEmployee = await _hrmsDbContext.Employees.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var empCode = await GenerateEmployeeCode(previousEmployee?.EmpCode);

            employee.EmpCode = empCode;
            employee.IsActive = true;
            employee.CreatedAt = DateTime.UtcNow;

            await _hrmsDbContext.Employees.AddAsync(employee);
            return employee;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = await _hrmsDbContext.Employees.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (existingEmployee == null)
            {
                _logger.LogWarning("Employee not found!", employee.Name);
                throw new Exception("Employee not found");
            }
            var duplicate = await _hrmsDbContext.Employees
                .AnyAsync(x => x.Id != id
                            && x.IsActive
                            && (x.Name == employee.Name
                                || x.Email == employee.Email
                                || x.PhoneNumber == employee.PhoneNumber));


            if (duplicate)
                throw new InvalidOperationException("An employee with this name already exists.");


            existingEmployee.Name = employee.Name;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.JoiningDate = employee.JoiningDate;
            existingEmployee.DateOfBirth = employee.DateOfBirth;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.DesignationId = employee.DesignationId;
            existingEmployee.EmploymentStatusId = employee.EmploymentStatusId;
            existingEmployee.UpdatedBy = employee.UpdatedBy;
            existingEmployee.UpdatedAt = DateTime.UtcNow;

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


            return true;
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