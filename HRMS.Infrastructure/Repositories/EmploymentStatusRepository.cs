using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Repositories
{
    public class EmploymentStatusRepository:IEmploymentStatusRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;
        private readonly ILogger<EmploymentStatusRepository> _logger;

        public EmploymentStatusRepository(HRMSDbContext hrmsDbContext, ILogger<EmploymentStatusRepository> logger)
        {
            _hrmsDbContext = hrmsDbContext;
            _logger = logger;
        }

        public async Task<(List<EmploymentStatus>, int totalCount)> GetAllEmploymentStatusAsync(QueryParameters parameters)
        {
            var query = _hrmsDbContext.EmploymentStatuses.Where(e => e.IsActive);

            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(e =>
                    e.StatusName.Contains(parameters.Search));

            query = parameters.SortBy?.ToLower() switch
            {
                "EmploymentStatusRepository" => parameters.SortDirection == "desc"
                        ? query.OrderByDescending(e => e.StatusName)
                        : query.OrderBy(e => e.StatusName),
                _ => query.OrderBy(e => e.Id),
            };

            var totalCount = await query.CountAsync();

            var EmploymentStatus = await query
                                .Skip((parameters.PageNumber - 1) * totalCount)
                                .Take(parameters.PageSize)
                                .ToListAsync();
            return (EmploymentStatus, totalCount);

        }

        public async Task<EmploymentStatus?> GetEmploymentStatusByIdAsync(int id)
        {
            return await _hrmsDbContext.EmploymentStatuses.Where(e => e.Id == id && e.IsActive).FirstOrDefaultAsync();
        }

        public async Task<EmploymentStatus> CreateEmploymentStatusAsync(EmploymentStatus EmploymentStatus)
        {

            if (EmploymentStatus == null)
            {
                throw new InvalidOperationException("EmploymentStatus data is null");
            }
            var existingEmploymentStatus = await _hrmsDbContext.EmploymentStatuses.Where(x => (x.StatusName == EmploymentStatus.StatusName) && x.IsActive).FirstOrDefaultAsync();
            if (existingEmploymentStatus != null)
            {
                _logger.LogWarning("EmploymentStatus already found!", existingEmploymentStatus.StatusName);
                throw new InvalidOperationException("EmploymentStatus with this email or phone already exists");
            }
            var previousEmploymentStatus = await _hrmsDbContext.EmploymentStatuses.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            EmploymentStatus.IsActive = true;
            EmploymentStatus.CreatedAt = DateTime.UtcNow;

            await _hrmsDbContext.EmploymentStatuses.AddAsync(EmploymentStatus);
            return EmploymentStatus;
        }

        public async Task<EmploymentStatus> UpdateEmploymentStatusAsync(int id, EmploymentStatus EmploymentStatus)
        {
            var existingEmploymentStatus = await _hrmsDbContext.EmploymentStatuses.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (existingEmploymentStatus == null)
            {
                _logger.LogWarning("EmploymentStatus not found!", existingEmploymentStatus.StatusName);
                throw new Exception("EmploymentStatus not found");
            }

            var duplicate = await _hrmsDbContext.EmploymentStatuses
                                    .AnyAsync(x => x.StatusName == EmploymentStatus.StatusName
                                                && x.Id != id
                                                && x.IsActive);

            if (duplicate)
                throw new InvalidOperationException("An employment status with this name already exists.");

            existingEmploymentStatus.StatusName = EmploymentStatus.StatusName;
            existingEmploymentStatus.StatusDisplayName = EmploymentStatus.StatusDisplayName;
            existingEmploymentStatus.UpdatedBy = EmploymentStatus.UpdatedBy;
            existingEmploymentStatus.UpdatedAt = DateTime.UtcNow;

            return existingEmploymentStatus;
        }

        public async Task<bool> DeleteEmploymentStatusAsync(int id, int userId)
        {
            var EmploymentStatus = await _hrmsDbContext.EmploymentStatuses.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (EmploymentStatus == null)
            {
                return false;
            }
            EmploymentStatus.IsActive = false;
            EmploymentStatus.UpdatedAt = DateTime.UtcNow;
            EmploymentStatus.UpdatedBy = userId;

            return true;
        }
    }
}
