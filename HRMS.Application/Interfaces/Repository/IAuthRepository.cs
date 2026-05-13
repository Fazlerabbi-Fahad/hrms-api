using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces.Repository
{
    public interface IAuthRepository
    {
        Task<User> GetByUserNameAsync(string UserName);

        Task<bool> RegisterAsync(User dto);
    }
}