using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Repositories
{
    public class DesignationRepository:IDesignationRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;
        private readonly ILogger<DesignationRepository> _logger;

        public DesignationRepository(HRMSDbContext hrmsDbContext, ILogger<DesignationRepository> logger)
        {
            _hrmsDbContext = hrmsDbContext;
            _logger = logger;
        }

        public async Task<(List<Designation>, int totalCount)> GetAllDesignationsAsync(QueryParameters parameters)
        {
            var query = _hrmsDbContext.Designations.Where(e => e.IsActive);

            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(e =>
                    e.DesignationName.Contains(parameters.Search));

            query = parameters.SortBy?.ToLower() switch
            {
                "DesignationRepository" => parameters.SortDirection == "desc"
                        ? query.OrderByDescending(e => e.DesignationName)
                        : query.OrderBy(e => e.DesignationName),
                _ => query.OrderBy(e => e.Id),
            };

            var totalCount = await query.CountAsync();

            var Designation = await query
                                .Skip((parameters.PageNumber - 1) * totalCount)
                                .Take(parameters.PageSize)
                                .ToListAsync();
            return (Designation, totalCount);

        }

        public async Task<Designation?> GetDesignationByIdAsync(int id)
        {
            return await _hrmsDbContext.Designations.Where(e => e.Id == id && e.IsActive).FirstOrDefaultAsync();
        }

        public async Task<Designation> CreateDesignationAsync(Designation Designation)
        {

            if (Designation == null)
            {
                throw new InvalidOperationException("Designation data is null");
            }
            var existingDesignation = await _hrmsDbContext.Designations.Where(x => (x.DesignationName == Designation.DesignationName) && x.IsActive).FirstOrDefaultAsync();
            if (existingDesignation != null)
            {
                _logger.LogWarning("Designation already found!", Designation.DesignationName);
                throw new InvalidOperationException("Designation with this email or phone already exists");
            }
            var previousDesignation = await _hrmsDbContext.Designations.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            Designation.IsActive = true;
            Designation.CreatedAt = DateTime.UtcNow;

            await _hrmsDbContext.Designations.AddAsync(Designation);
            return Designation;
        }

        public async Task<Designation> UpdateDesignationAsync(int id, Designation Designation)
        {
            var existingDesignation = await _hrmsDbContext.Designations.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (existingDesignation == null)
            {
                _logger.LogWarning("Designation not found!", Designation.DesignationName);
                throw new Exception("Designation not found");
            }

            var duplicate = await _hrmsDbContext.Departments
                                                .AnyAsync(x => x.DepartmentName == Designation.DesignationName
                                                            && x.Id != id
                                                            && x.IsActive);

            if (duplicate)
                throw new InvalidOperationException("A designation with this name already exists.");

            existingDesignation.DesignationName= Designation.DesignationName;
            existingDesignation.DesignationDisplayName = Designation.DesignationDisplayName;
            existingDesignation.UpdatedBy = Designation.UpdatedBy;
            existingDesignation.UpdatedAt = DateTime.UtcNow;

            return existingDesignation;
        }

        public async Task<bool> DeleteDesignationAsync(int id, int userId)
        {
            var Designation = await _hrmsDbContext.Designations.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (Designation == null)
            {
                return false;
            }
            Designation.IsActive = false;
            Designation.UpdatedAt = DateTime.UtcNow;
            Designation.UpdatedBy = userId;

            return true;
        }

    }
}
