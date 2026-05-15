using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.PaymentStatus;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Application.Mappers;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class PaymentStatusService:IPaymentStatusService
    {
        private readonly IPaymentStatusRepository _PaymentStatusRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<PaymentStatusService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentStatusService(IPaymentStatusRepository PaymentStatusRepository, ICacheService cacheService, ILogger<PaymentStatusService> logger, IUnitOfWork unitOfWork)
        {
            _PaymentStatusRepository = PaymentStatusRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<PaymentStatusResponseDto>>> GetAllPaymentStatussAsync(PaymentStatusQueryParameters parameters)
        {
            var cacheKey = CacheKeys.PaymentStatusList(
                    parameters.PageNumber,
                    parameters.PageSize,
                    parameters.Search
                );


            var cached = _cacheService.Get<PagedResult<PaymentStatusResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("PaymentStatuss retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<PagedResult<PaymentStatusResponseDto>>.Success(cached,
                                    $"Retrieved PaymentStatuss successfully!"
                            );
            }

            var (PaymentStatuses, totalCoount) = await _PaymentStatusRepository.GetAllPaymentStatusAsync(parameters);

            var PaymentStatusDtos = PaymentStatusMapper.ToResponseDtoList(PaymentStatuses);

            var pagedResult = new PagedResult<PaymentStatusResponseDto>
            {
                Items = PaymentStatusDtos,
                TotalCount = totalCoount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            _cacheService.Set(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return ApiResponse<PagedResult<PaymentStatusResponseDto>>.Success(pagedResult, AppConstants.Messages.Success);
        }

        public async Task<ApiResponse<PaymentStatusResponseDto>> GetPaymentStatusByIdAsync(int id)
        {
            var cacheKey = CacheKeys.PaymentStatusById(id);


            var cached = _cacheService.Get<PaymentStatusResponseDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("PaymentStatus retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<PaymentStatusResponseDto>.Success(cached, AppConstants.Messages.Success);
            }

            var PaymentStatus = await _PaymentStatusRepository.GetPaymentStatusByIdAsync(id);

            var PaymentStatusDto = PaymentStatusMapper.ToResponseDto(PaymentStatus);

            _cacheService.Set(cacheKey, PaymentStatusDto, TimeSpan.FromMinutes(5));

            return ApiResponse<PaymentStatusResponseDto>.Success(PaymentStatusDto,
                                    PaymentStatusDto != null ? AppConstants.Messages.Success
                                        : AppConstants.Messages.NotFound
                                    );
        }

        public async Task<ApiResponse<PaymentStatusResponseDto>> CreatePaymentStatusAsync(PaymentStatusRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newPaymentStatus = new PaymentStatus
                {
                    StatusName = dto.StatusName,
                    StatusDisplayName = dto.StatusName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.Id
                };
                var PaymentStatus = await _unitOfWork.PaymentStatuses.CreatePaymentStatusAsync(newPaymentStatus);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.PaymentStatusList(1, int.MaxValue, null));

                var createdPaymentStatusDto = PaymentStatusMapper.ToResponseDto(PaymentStatus);

                _logger.LogInformation("PaymentStatus created by user {UserId}", dto.Id);

                return ApiResponse<PaymentStatusResponseDto>.Success(createdPaymentStatusDto,
                                        PaymentStatus != null ? AppConstants.Messages.Success
                                            : AppConstants.Messages.NotFound
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating PaymentStatus by user {UserId}", dto.Id);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<PaymentStatusResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<PaymentStatusResponseDto>> UpdatePaymentStatusAsync(int id, PaymentStatusUpdateRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updatePaymentStatusDto = new PaymentStatus
                {
                    StatusName = dto.StatusName,
                    StatusDisplayName = dto.StatusName,
                    UpdatedBy = dto.UserId
                };

                var PaymentStatus = await _unitOfWork.PaymentStatuses.UpdatePaymentStatusAsync(id, updatePaymentStatusDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.PaymentStatusList(1, int.MaxValue, null));

                var updatedPaymentStatusDto = PaymentStatusMapper.ToResponseDto(PaymentStatus);
                return ApiResponse<PaymentStatusResponseDto>.Success(updatedPaymentStatusDto,
                                        PaymentStatus != null ? "PaymentStatus updated successfully!"
                                            : "PaymentStatus update failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating PaymentStatus with id {PaymentStatusId} by user {UserId}", id, dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<PaymentStatusResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<bool>> DeletePaymentStatusAsync(int id, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isDeleted = await _unitOfWork.PaymentStatuses.DeletePaymentStatusAsync(id, userId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.PaymentStatusList(1, int.MaxValue, null));

                _logger.LogInformation("PaymentStatus deleted by user {UserId}", userId);

                return ApiResponse<bool>.Success(isDeleted,
                                        isDeleted ? "PaymentStatus deleted successfully!"
                                            : "PaymentStatus deletion failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting PaymentStatus with id {PaymentStatusId} by user {UserId}", id, userId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }
    }
}
