using HRMS.Application.DTOs.Auth;
using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;

namespace HRMS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
        {
            try
            {
                var user = await _authRepository.GetByUserNameAsync(dto.Username);
                if (user == null)
                {
                    return ApiResponse<LoginResponseDto>.Failure(null, "Invalid username or password.", 401);
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    return ApiResponse<LoginResponseDto>.Failure(null, "Invalid username or password.", 401);
                }

                if (!user.IsActive)
                {
                    return ApiResponse<LoginResponseDto>.Failure(null, "User account is deactivate.", 403);
                }

                var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

                var token = _tokenService.GenerateToken(user, roles);

                await _authRepository.UpdateLastLoginAsync(user.Id);

                var responseDto = new LoginResponseDto
                {
                    Token = token,
                    UserName = user.UserName,
                    Roles = roles,
                    ExpiresAt = DateTime.UtcNow.AddHours(168)
                };
                return ApiResponse<LoginResponseDto>.Success(responseDto, "Login successful.");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Failure(new List<string> { ex.Message }, "An error occurred during login.", 500);
            }
        }
    }
}