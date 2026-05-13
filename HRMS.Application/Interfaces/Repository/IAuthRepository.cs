using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<User?> GetByUserNameAsync(string UserName);

        Task<User> GetByIdAsync(int id);

        Task<User> CreateUserAsync(User user);

        Task UpdateLastLoginAsync(int userId);
    }
}