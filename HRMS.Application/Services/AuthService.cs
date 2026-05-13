using HRMS.Application.DTOs.Auth;
using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;

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
                    return ApiResponse<LoginResponseDto>.Failure(null, "Invalid username or password!", 401);
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    return ApiResponse<LoginResponseDto>.Failure(null, "Invalid username or password!", 401);
                }

                if (!user.IsActive)
                {
                    return ApiResponse<LoginResponseDto>.Failure(null, "User account is deactivate!", 403);
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
                return ApiResponse<LoginResponseDto>.Success(responseDto, "Login successful!");
            }
            catch (Exception ex)
            {
                return ApiResponse<LoginResponseDto>.Failure(new List<string> { ex.Message }, "An error occurred during login!", 500);
            }
        }

        public async Task<ApiResponse<bool>> RegisterAsync(RegisterRequestDto dto)
        {
            try
            {
                var existingUser = await _authRepository.GetByUserNameAsync(dto.Username);
                if (existingUser != null)
                {
                    return ApiResponse<bool>.Failure(null, "Username already exists!", 409);
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    UserName = dto.Username,
                    PasswordHash = passwordHash,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,

                    UserRoles = dto.RoleIds.Select(roleId => new UserRole
                    {
                        RoleId = roleId,
                        AssignedAt = DateTime.UtcNow
                    }).ToList()
                };

                await _authRepository.CreateUserAsync(user);

                return ApiResponse<bool>.Success(true, "Registration successful!", 201);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(new List<string> { ex.Message }, "An error occurred during registration.", 500);
            }
        }
    }
}