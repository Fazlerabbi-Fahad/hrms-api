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
            var user = await _hrmsDbContext.Users.Where(u => u.UserName == username)
                                                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}