using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Repositories
{
    public class DepartmentRepository:IDepartmentRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(HRMSDbContext hrmsDbContext, ILogger<DepartmentRepository> logger)
        {
            _hrmsDbContext = hrmsDbContext;
            _logger = logger;
        }

        public async Task<(List<Department>, int totalCount)> GetAllDepartmentsAsync(QueryParameters parameters)
        {
            var query = _hrmsDbContext.Departments.Where(e => e.IsActive);

            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(e =>
                    e.DepartmentName.Contains(parameters.Search));

            query = parameters.SortBy?.ToLower() switch
            {
                "departmentRepository" => parameters.SortDirection == "desc"
                        ? query.OrderByDescending(e => e.DepartmentName)
                        : query.OrderBy(e => e.DepartmentName),
                _ => query.OrderBy(e => e.Id),
            };

            var totalCount = await query.CountAsync();

            var Department = await query
                                .Skip((parameters.PageNumber - 1) * totalCount)
                                .Take(parameters.PageSize)
                                .ToListAsync();
            return (Department, totalCount);

        }

        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _hrmsDbContext.Departments.Where(e => e.Id == id && e.IsActive).FirstOrDefaultAsync();
        }

        public async Task<Department> CreateDepartmentAsync(Department Department)
        {

            if (Department == null)
            {
                throw new InvalidOperationException("Department data is null");
            }
            var existingDepartment = await _hrmsDbContext.Departments.Where(x => (x.DepartmentName == Department.DepartmentName) && x.IsActive).FirstOrDefaultAsync();
            if (existingDepartment != null)
            {
                _logger.LogWarning("Department already found!", Department.DepartmentName);
                throw new InvalidOperationException("Department with this email or phone already exists");
            }
            var previousDepartment = await _hrmsDbContext.Departments.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            Department.IsActive = true;
            Department.CreatedAt = DateTime.UtcNow;

            await _hrmsDbContext.Departments.AddAsync(Department);
            return Department;
        }

        public async Task<Department> UpdateDepartmentAsync(int id, Department department)
        {
            var existingDepartment = await _hrmsDbContext.Departments
                                                         .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

            if (existingDepartment == null)
            {
                _logger.LogWarning("Department not found!");
                throw new Exception("Department not found");
            }

            var duplicate = await _hrmsDbContext.Departments
                                                .AnyAsync(x => x.DepartmentName == department.DepartmentName
                                                            && x.Id != id
                                                            && x.IsActive);

            if (duplicate)
                throw new InvalidOperationException("A department with this name already exists.");


            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.DepartmentDisplayName = department.DepartmentDisplayName;
            existingDepartment.UpdatedBy = department.UpdatedBy;
            existingDepartment.UpdatedAt = DateTime.UtcNow;

            return existingDepartment;
        }

        public async Task<bool> DeleteDepartmentAsync(int id, int userId)
        {
            var Department = await _hrmsDbContext.Departments.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (Department == null)
            {
                return false;
            }
            Department.IsActive = false;
            Department.UpdatedAt = DateTime.UtcNow;
            Department.UpdatedBy = userId;

            return true;
        }

    }
}
