namespace HRMS.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string email, List<string> roles);
    }
}
