using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Designation;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Application.Mappers;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class DesignationService:IDesignationService
    {
        private readonly IDesignationRepository _DesignationRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DesignationService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DesignationService(IDesignationRepository DesignationRepository, ICacheService cacheService, ILogger<DesignationService> logger, IUnitOfWork unitOfWork)
        {
            _DesignationRepository = DesignationRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<DesignationResponseDto>>> GetAllDesignationsAsync(QueryParameters parameters)
        {
            //var cacheKey = CacheKeys.DesignationList(
            //        parameters.PageNumber,
            //        parameters.PageSize,
            //        parameters.Search
            //    );


            //var cached = _cacheService.Get<PagedResult<DesignationResponseDto>>(cacheKey);
            //if (cached != null)
            //{
            //    _logger.LogInformation("Designations retrieved from cache with key {CacheKey}", cacheKey);
            //    return ApiResponse<PagedResult<DesignationResponseDto>>.Success(cached,
            //                        $"Retrieved Designations successfully!"
            //                );
            //}

            var (Designations, totalCoount) = await _DesignationRepository.GetAllDesignationsAsync(parameters);

            var DesignationDtos = DesignationMapper.ToResponseDtoList(Designations);

            var pagedResult = new PagedResult<DesignationResponseDto>
            {
                Items = DesignationDtos,
                TotalCount = totalCoount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            //_cacheService.Set(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return ApiResponse<PagedResult<DesignationResponseDto>>.Success(pagedResult, AppConstants.Messages.Success);
        }

        public async Task<ApiResponse<DesignationResponseDto>> GetDesignationByIdAsync(int id)
        {
            var cacheKey = CacheKeys.DesignationById(id);


            var cached = _cacheService.Get<DesignationResponseDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Designation retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<DesignationResponseDto>.Success(cached, AppConstants.Messages.Success);
            }

            var Designation = await _DesignationRepository.GetDesignationByIdAsync(id);

            var DesignationDto = DesignationMapper.ToResponseDto(Designation);

            _cacheService.Set(cacheKey, DesignationDto, TimeSpan.FromMinutes(5));

            return ApiResponse<DesignationResponseDto>.Success(DesignationDto,
                                    DesignationDto != null ? AppConstants.Messages.Success
                                        : AppConstants.Messages.NotFound
                                    );
        }

        public async Task<ApiResponse<DesignationResponseDto>> CreateDesignationAsync(DesignationRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newDesignation = new Designation
                {
                    DesignationName = dto.DesignationName,
                    DesignationDisplayName = dto.DesignationName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId
                };
                var Designation = await _unitOfWork.Designations.CreateDesignationAsync(newDesignation);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.DesignationList(1, int.MaxValue, null));

                var createdDesignationDto = DesignationMapper.ToResponseDto(Designation);

                _logger.LogInformation("Designation created by user {UserId}", dto.UserId);

                return ApiResponse<DesignationResponseDto>.Success(createdDesignationDto,
                                        Designation != null ? AppConstants.Messages.Success
                                            : AppConstants.Messages.NotFound
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Designation by user {UserId}", dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<DesignationResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<DesignationResponseDto>> UpdateDesignationAsync(int id, DesignationUpdateRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updateDesignationDto = new Designation
                {
                    DesignationName = dto.DesignationName,
                    DesignationDisplayName = dto.DesignationName,
                    UpdatedBy = dto.UserId
                };

                var Designation = await _unitOfWork.Designations.UpdateDesignationAsync(id, updateDesignationDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.DesignationList(1, int.MaxValue, null));

                var updatedDesignationDto = DesignationMapper.ToResponseDto(Designation);
                return ApiResponse<DesignationResponseDto>.Success(updatedDesignationDto,
                                        Designation != null ? "Designation updated successfully!"
                                            : "Designation update failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Designation with id {DesignationId} by user {UserId}", id, dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<DesignationResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteDesignationAsync(int id, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isDeleted = await _unitOfWork.Designations.DeleteDesignationAsync(id, userId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.DesignationList(1, int.MaxValue, null));

                _logger.LogInformation("Designation deleted by user {UserId}", userId);

                return ApiResponse<bool>.Success(isDeleted,
                                        isDeleted ? "Designation deleted successfully!"
                                            : "Designation deletion failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Designation with id {DesignationId} by user {UserId}", id, userId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }
    }
}
