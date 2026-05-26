using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Role;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Application.Mappers;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class RoleService: IRoleService
    {
        private readonly IRoleRepository _RoleRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<RoleService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IRoleRepository RoleRepository, ICacheService cacheService, ILogger<RoleService> logger, IUnitOfWork unitOfWork)
        {
            _RoleRepository = RoleRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<RoleResponseDto>>> GetAllRolesAsync(QueryParameters parameters)
        {
            //var cacheKey = CacheKeys.RoleList(
            //        parameters.PageNumber,
            //        parameters.PageSize,
            //        parameters.Search
            //    );


            //var cached = _cacheService.Get<PagedResult<RoleResponseDto>>(cacheKey);
            //if (cached != null)
            //{
            //    _logger.LogInformation("Roles retrieved from cache with key {CacheKey}", cacheKey);
            //    return ApiResponse<PagedResult<RoleResponseDto>>.Success(cached,
            //                        $"Retrieved Roles successfully!"
            //                );
            //}

            var (Rolees, totalCoount) = await _RoleRepository.GetAllRoleAsync(parameters);

            var RoleDtos = RoleMapper.ToResponseDtoList(Rolees);

            var pagedResult = new PagedResult<RoleResponseDto>
            {
                Items = RoleDtos,
                TotalCount = totalCoount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            //_cacheService.Set(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return ApiResponse<PagedResult<RoleResponseDto>>.Success(pagedResult, AppConstants.Messages.Success);
        }

        public async Task<ApiResponse<RoleResponseDto>> GetRoleByIdAsync(int id)
        {
            var cacheKey = CacheKeys.RoleById(id);


            var cached = _cacheService.Get<RoleResponseDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Role retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<RoleResponseDto>.Success(cached, AppConstants.Messages.Success);
            }

            var Role = await _RoleRepository.GetRoleByIdAsync(id);

            var RoleDto = RoleMapper.ToResponseDto(Role);

            _cacheService.Set(cacheKey, RoleDto, TimeSpan.FromMinutes(5));

            return ApiResponse<RoleResponseDto>.Success(RoleDto,
                                    RoleDto != null ? AppConstants.Messages.Success
                                        : AppConstants.Messages.NotFound
                                    );
        }

        public async Task<ApiResponse<RoleResponseDto>> CreateRoleAsync(RoleRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newRole = new Role
                {
                    RoleName = dto.RoleName,
                    RoleDisplayName= dto.RoleName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.Id
                };
                var Role = await _unitOfWork.Roles.CreateRoleAsync(newRole);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.RoleList(1, int.MaxValue, null));

                var createdRoleDto = RoleMapper.ToResponseDto(Role);

                _logger.LogInformation("Role created by user {UserId}", dto.Id);

                return ApiResponse<RoleResponseDto>.Success(createdRoleDto,
                                        Role != null ? AppConstants.Messages.Success
                                            : AppConstants.Messages.NotFound
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Role by user {UserId}", dto.Id);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<RoleResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<RoleResponseDto>> UpdateRoleAsync(int id, RoleUpdateRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updateRoleDto = new Role
                {
                    RoleName = dto.RoleName,
                    RoleDisplayName = dto.RoleName,
                    UpdatedBy = dto.UserId
                };

                var Role = await _unitOfWork.Roles.UpdateRoleAsync(id, updateRoleDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.RoleList(1, int.MaxValue, null));

                var updatedRoleDto = RoleMapper.ToResponseDto(Role);
                return ApiResponse<RoleResponseDto>.Success(updatedRoleDto,
                                        Role != null ? "Role updated successfully!"
                                            : "Role update failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Role with id {RoleId} by user {UserId}", id, dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<RoleResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteRoleAsync(int id, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isDeleted = await _unitOfWork.Roles.DeleteRoleAsync(id, userId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.RoleList(1, int.MaxValue, null));

                _logger.LogInformation("Role deleted by user {UserId}", userId);

                return ApiResponse<bool>.Success(isDeleted,
                                        isDeleted ? "Role deleted successfully!"
                                            : "Role deletion failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Role with id {RoleId} by user {UserId}", id, userId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }
    }
}
