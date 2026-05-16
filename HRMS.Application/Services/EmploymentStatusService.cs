using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.EmploymentStatus;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Application.Mappers;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class EmploymentStatusService:IEmploymentStatusService
    {
        private readonly IEmploymentStatusRepository _EmploymentStatusRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<EmploymentStatusService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public EmploymentStatusService(IEmploymentStatusRepository EmploymentStatusRepository, ICacheService cacheService, ILogger<EmploymentStatusService> logger, IUnitOfWork unitOfWork)
        {
            _EmploymentStatusRepository = EmploymentStatusRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<EmploymentStatusResponseDto>>> GetAllEmploymentStatussAsync(QueryParameters parameters)
        {
            var cacheKey = CacheKeys.EmploymentStatusList(
                    parameters.PageNumber,
                    parameters.PageSize,
                    parameters.Search
                );


            var cached = _cacheService.Get<PagedResult<EmploymentStatusResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("EmploymentStatuss retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<PagedResult<EmploymentStatusResponseDto>>.Success(cached,
                                    $"Retrieved EmploymentStatuss successfully!"
                            );
            }

            var (EmploymentStatuss, totalCoount) = await _EmploymentStatusRepository.GetAllEmploymentStatusAsync(parameters);

            var EmploymentStatusDtos = EmploymentStatusMapper.ToResponseDtoList(EmploymentStatuss);

            var pagedResult = new PagedResult<EmploymentStatusResponseDto>
            {
                Items = EmploymentStatusDtos,
                TotalCount = totalCoount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            _cacheService.Set(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return ApiResponse<PagedResult<EmploymentStatusResponseDto>>.Success(pagedResult, AppConstants.Messages.Success);
        }

        public async Task<ApiResponse<EmploymentStatusResponseDto>> GetEmploymentStatusByIdAsync(int id)
        {
            var cacheKey = CacheKeys.EmploymentStatusById(id);


            var cached = _cacheService.Get<EmploymentStatusResponseDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("EmploymentStatus retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<EmploymentStatusResponseDto>.Success(cached, AppConstants.Messages.Success);
            }

            var EmploymentStatus = await _EmploymentStatusRepository.GetEmploymentStatusByIdAsync(id);

            var EmploymentStatusDto = EmploymentStatusMapper.ToResponseDto(EmploymentStatus);

            _cacheService.Set(cacheKey, EmploymentStatusDto, TimeSpan.FromMinutes(5));

            return ApiResponse<EmploymentStatusResponseDto>.Success(EmploymentStatusDto,
                                    EmploymentStatusDto != null ? AppConstants.Messages.Success
                                        : AppConstants.Messages.NotFound
                                    );
        }

        public async Task<ApiResponse<EmploymentStatusResponseDto>> CreateEmploymentStatusAsync(EmploymentStatusRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newEmploymentStatus = new EmploymentStatus
                {
                    StatusName = dto.StatusName,
                    StatusDisplayName = dto.StatusName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId
                };
                var EmploymentStatus = await _unitOfWork.EmploymentStatuses.CreateEmploymentStatusAsync(newEmploymentStatus);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.EmploymentStatusList(1, int.MaxValue, null));

                var createdEmploymentStatusDto = EmploymentStatusMapper.ToResponseDto(EmploymentStatus);

                _logger.LogInformation("EmploymentStatus created by user {UserId}", dto.UserId);

                return ApiResponse<EmploymentStatusResponseDto>.Success(createdEmploymentStatusDto,
                                        EmploymentStatus != null ? AppConstants.Messages.Success
                                            : AppConstants.Messages.NotFound
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating EmploymentStatus by user {UserId}", dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<EmploymentStatusResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<EmploymentStatusResponseDto>> UpdateEmploymentStatusAsync(int id, EmploymentStatusUpdateRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updateEmploymentStatusDto = new EmploymentStatus
                {
                    StatusName = dto.StatusName,
                    StatusDisplayName = dto.StatusName,
                    UpdatedBy = dto.UserId
                };

                var EmploymentStatus = await _unitOfWork.EmploymentStatuses.UpdateEmploymentStatusAsync(id, updateEmploymentStatusDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.EmploymentStatusList(1, int.MaxValue, null));

                var updatedEmploymentStatusDto = EmploymentStatusMapper.ToResponseDto(EmploymentStatus);
                return ApiResponse<EmploymentStatusResponseDto>.Success(updatedEmploymentStatusDto,
                                        EmploymentStatus != null ? "EmploymentStatus updated successfully!"
                                            : "EmploymentStatus update failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating EmploymentStatus with id {EmploymentStatusId} by user {UserId}", id, dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<EmploymentStatusResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteEmploymentStatusAsync(int id, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isDeleted = await _unitOfWork.EmploymentStatuses.DeleteEmploymentStatusAsync(id, userId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.EmploymentStatusList(1, int.MaxValue, null));

                _logger.LogInformation("EmploymentStatus deleted by user {UserId}", userId);

                return ApiResponse<bool>.Success(isDeleted,
                                        isDeleted ? "EmploymentStatus deleted successfully!"
                                            : "EmploymentStatus deletion failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting EmploymentStatus with id {EmploymentStatusId} by user {UserId}", id, userId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }
    }
}
