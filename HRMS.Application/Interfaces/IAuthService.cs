using HRMS.Application.DTOs.Auth;
using HRMS.Application.DTOs.Common;

namespace HRMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto);
        Task<ApiResponse<bool>> RegisterAsync(RegisterRequestDto dto);
    }
}
