using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HRMS.Infrastructure.Repositories
{
    public class RoleRepository: IRoleRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(HRMSDbContext hrmsDbContext, ILogger<RoleRepository> logger)
        {
            _hrmsDbContext = hrmsDbContext;
            _logger = logger;
        }

        public async Task<(List<Role>, int totalCount)> GetAllRoleAsync(QueryParameters parameters)
        {
            var query = _hrmsDbContext.Roles.Where(e => e.IsActive);

            if (!string.IsNullOrEmpty(parameters.Search))
                query = query.Where(e =>
                    e.RoleName.Contains(parameters.Search));

            query = parameters.SortBy?.ToLower() switch
            {
                "rolerepository" => parameters.SortDirection == "desc"
                        ? query.OrderByDescending(e => e.RoleName)
                        : query.OrderBy(e => e.RoleName),
                _ => query.OrderBy(e => e.Id),
            };

            var totalCount = await query.CountAsync();

            var Role = await query
                                .Skip((parameters.PageNumber - 1) * totalCount)
                                .Take(parameters.PageSize)
                                .ToListAsync();
            return (Role, totalCount);

        }

        public async Task<Role?> GetRoleByIdAsync(int id)
        {
            return await _hrmsDbContext.Roles.Where(e => e.Id == id && e.IsActive).FirstOrDefaultAsync();
        }

        public async Task<Role> CreateRoleAsync(Role Role)
        {

            if (Role == null)
            {
                throw new InvalidOperationException("Role data is null");
            }
            var existingRole = await _hrmsDbContext.Roles.Where(x => (x.RoleName == Role.RoleName) && x.IsActive).FirstOrDefaultAsync();
            if (existingRole != null)
            {
                _logger.LogWarning("Role already found!", existingRole.RoleName);
                throw new InvalidOperationException("Role with this email or phone already exists");
            }
            var previousRole = await _hrmsDbContext.Roles.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            Role.IsActive = true;
            Role.CreatedAt = DateTime.UtcNow;

            await _hrmsDbContext.Roles.AddAsync(Role);
            return Role;
        }

        public async Task<Role> UpdateRoleAsync(int id, Role Role)
        {
            var existingRole = await _hrmsDbContext.Roles.Where(x => x.Id == id && x.IsActive).FirstOrDefaultAsync();
            if (existingRole == null)
            {
                _logger.LogWarning("Role not found!", existingRole.RoleName);
                throw new Exception("Role not found");
            }

            var duplicate = await _hrmsDbContext.Roles
                                    .AnyAsync(x => x.RoleName == Role.RoleName
                                                && x.Id != id
                                                && x.IsActive);

            if (duplicate)
                throw new InvalidOperationException("A role with this name already exists.");

            existingRole.RoleName=Role.RoleName;
            existingRole.RoleDisplayName=Role.RoleDisplayName;
            existingRole.UpdatedBy = Role.UpdatedBy;
            existingRole.UpdatedAt = DateTime.UtcNow;

            return existingRole;
        }

        public async Task<bool> DeleteRoleAsync(int id, int userId)
        {
            var Role = await _hrmsDbContext.Roles.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (Role == null)
            {
                return false;
            }
            Role.IsActive = false;
            Role.UpdatedAt = DateTime.UtcNow;
            Role.UpdatedBy = userId;

            return true;
        }
    }
}
