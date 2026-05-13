using HRMS.Domain.Entities;

namespace HRMS.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user, List<string> roles);
    }
}