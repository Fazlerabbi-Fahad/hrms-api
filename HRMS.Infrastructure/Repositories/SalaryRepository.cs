using HRMS.Application.DTOs.Salary;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Repositories
{
    public class SalaryRepository:ISalaryRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;
        private readonly ILogger<SalaryRepository> _logger;

        public SalaryRepository(HRMSDbContext hrmsDbContext, ILogger<SalaryRepository> logger)
        {
            _hrmsDbContext = hrmsDbContext;
            _logger = logger;
        }

        public async Task<(List<Salary>, int totalCount)> GetAllSalaryAsync(SalaryQueryParameters parameters)
        {
            var query = _hrmsDbContext.Salaries.Where(e => e.IsActive);

            if (parameters.EmployeeId.HasValue)
            {
                query = query.Where(e => e.EmployeeId == parameters.EmployeeId.Value);
            }

            if (parameters.EffectiveFrom.HasValue)
            {
                query = query.Where(e => e.EffectiveFrom >= parameters.EffectiveFrom.Value);
            }

            if (parameters.EffectiveTo.HasValue)
            {
                query = query.Where(e => e.EffectiveTo <= parameters.EffectiveTo.Value);
            }

            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(e =>
                    e.BasicSalary.ToString().Contains(parameters.Search) ||
                    e.HouseAllowance.ToString().Contains(parameters.Search) ||
                    e.MedicalAllowance.ToString().Contains(parameters.Search) ||
                    e.TransportAllowance.ToString().Contains(parameters.Search));

            query = parameters.SortBy?.ToLower() switch
            {
                "basicsalary" => parameters.SortDirection == "desc"
                        ? query.OrderByDescending(e => e.BasicSalary)
                        : query.OrderBy(e => e.BasicSalary),
                "effectivefrom" => parameters.SortDirection == "desc"
                        ? query.OrderByDescending(e => e.EffectiveFrom)
                        : query.OrderBy(e => e.EffectiveFrom),
                _ => query.OrderBy(e => e.Id),
            };

            var totalCount = await query.CountAsync();

            var Salary = await query
                                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                                .Take(parameters.PageSize)
                                .ToListAsync();
            return (Salary, totalCount);

        }

        public async Task<Salary?> GetSalaryByIdAsync(int id)
        {
            return await _hrmsDbContext.Salaries
                .Where(e => e.Id == id && e.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<Salary> CreateSalaryAsync(Salary Salary)
        {

            if (Salary == null)
            {
                throw new InvalidOperationException("Salary data is null");
            }
            var existingSalary = await _hrmsDbContext.Salaries.Where(x => x.BasicSalary == Salary.BasicSalary
                                                                        && x.EmployeeId == Salary.EmployeeId
                                                                        && x.IsActive)
                                                    .FirstOrDefaultAsync();
            if (existingSalary != null)
            {
                _logger.LogWarning("Salary already found!", Salary.BasicSalary);
                throw new InvalidOperationException("Salary with this email or phone already exists");
            }
            var previousSalary = await _hrmsDbContext.Salaries.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            Salary.IsActive = true;
            Salary.CreatedAt = DateTime.UtcNow;

            await _hrmsDbContext.Salaries.AddAsync(Salary);
            return Salary;
        }

        public async Task<Salary> UpdateSalaryAsync(int id, Salary Salary)
        {
            var existingSalary = await _hrmsDbContext.Salaries.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (existingSalary == null)
            {
                _logger.LogWarning("Salary not found!", Salary.BasicSalary);
                throw new Exception("Salary not found");
            }

            existingSalary.UpdatedAt = DateTime.UtcNow;

            //await _hrmsDbContext.Salarys.UpdateAsync(Salary);
            return existingSalary;
        }

        public async Task<bool> DeleteSalaryAsync(int id, int userId)
        {
            var Salary = await _hrmsDbContext.Salaries.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (Salary == null)
            {
                return false;
            }
            Salary.IsActive = false;
            Salary.UpdatedAt = DateTime.UtcNow;
            Salary.UpdatedBy = userId;


            return true;
        }
    }
}
