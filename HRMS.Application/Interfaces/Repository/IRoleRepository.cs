using HRMS.Application.DTOs.Common;
using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IRoleRepository
    {
        Task<(List<Role>, int totalCount)> GetAllRoleAsync(QueryParameters parameters);
        Task<Role?> GetRoleByIdAsync(int id);
        Task<Role> CreateRoleAsync(Role Role);
        Task<Role> UpdateRoleAsync(int id, Role Role);
        Task<bool> DeleteRoleAsync(int id, int userId);
    }
}
