using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Data.HRMSDbContext;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HRMSDbContext _hrmsDbContext;

        public AuthRepository(HRMSDbContext hrmsDbContext)
        {
            _hrmsDbContext = hrmsDbContext;
        }

        public async Task<User?> GetByUserNameAsync(string username)
        {
            var user = await _hrmsDbContext.Users
                                            .Include(u => u.UserRoles)
                                                .ThenInclude(ur => ur.Role)
                                            .Where(u => u.UserName == username)
                                                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _hrmsDbContext.Users
                                            .Include(u => u.UserRoles)
                                                .ThenInclude(ur => ur.Role)
                                            .Where(u => u.Id == id)
                                                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _hrmsDbContext.Users.AddAsync(user);
            await _hrmsDbContext.SaveChangesAsync();

            return user;
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _hrmsDbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                _hrmsDbContext.Users.Update(user);
                await _hrmsDbContext.SaveChangesAsync();
            }
        }
    }
}