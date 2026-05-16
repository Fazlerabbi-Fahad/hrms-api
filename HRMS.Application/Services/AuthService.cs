using HRMS.Application.DTOs.Auth;
using HRMS.Application.DTOs.Common;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;


namespace HRMS.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IAuthRepository authRepository, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto dto)
        {
            var user = await _authRepository.GetByUserNameAsync(dto.Username);
            if (user == null)
            {
                return ApiResponse<LoginResponseDto>.Failure(null, "Invalid username or password!", 401);
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Failed login attempt for username {Username}", dto.Username);
                return ApiResponse<LoginResponseDto>.Failure(null, "Invalid username or password!", 401);
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Failed login attempt for username {Username}", dto.Username);
                return ApiResponse<LoginResponseDto>.Failure(null, "User account is deactivate!", 403);
            }

            var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

            var token = _tokenService.GenerateToken(user, roles);

            await _authRepository.UpdateLastLoginAsync(user.Id);
            _logger.LogInformation("User {Username} logged in successfully", dto.Username);

            var responseDto = new LoginResponseDto
            {
                Token = token,
                UserName = user.UserName,
                Roles = roles,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            };
            return ApiResponse<LoginResponseDto>.Success(responseDto, "Login successful!");
        }

        public async Task<ApiResponse<bool>> RegisterAsync(RegisterRequestDto dto)
        {

            var existingUser = await _authRepository.GetByUserNameAsync(dto.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("Failed registration attempt for username {Username}", dto.Username);
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
                    IsActive=true,
                    CreatedAt = DateTime.UtcNow,
                    AssignedAt = DateTime.UtcNow
                }).ToList()
            };

            await _authRepository.CreateUserAsync(user);
            _logger.LogInformation("User {Username} registered in successfully", dto.Username);

            return ApiResponse<bool>.Success(true, "Registration successful!", 201);
        }
    }
}