using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Salary;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Application.Mappers;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class SalaryService: ISalaryService
    {
        private readonly ISalaryRepository _SalaryRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<SalaryService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SalaryService(ISalaryRepository SalaryRepository, ICacheService cacheService, ILogger<SalaryService> logger, IUnitOfWork unitOfWork)
        {
            _SalaryRepository = SalaryRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<SalaryResponseDto>>> GetAllSalarysAsync(SalaryQueryParameters parameters)
        {
            var cacheKey = CacheKeys.SalaryList(
                    parameters.PageNumber,
                    parameters.PageSize,
                    parameters.Search
                );


            var cached = _cacheService.Get<PagedResult<SalaryResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Salarys retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<PagedResult<SalaryResponseDto>>.Success(cached,
                                    $"Retrieved Salarys successfully!"
                            );
            }

            var (Salarys, totalCoount) = await _SalaryRepository.GetAllSalaryAsync(parameters);

            var SalaryDtos = SalaryMapper.ToResponseDtoList(Salarys);

            var pagedResult = new PagedResult<SalaryResponseDto>
            {
                Items = SalaryDtos,
                TotalCount = totalCoount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            _cacheService.Set(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return ApiResponse<PagedResult<SalaryResponseDto>>.Success(pagedResult, AppConstants.Messages.Success);
        }

        public async Task<ApiResponse<SalaryResponseDto>> GetSalaryByIdAsync(int id)
        {
            var cacheKey = CacheKeys.SalaryById(id);


            var cached = _cacheService.Get<SalaryResponseDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Salary retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<SalaryResponseDto>.Success(cached, AppConstants.Messages.Success);
            }

            var Salary = await _SalaryRepository.GetSalaryByIdAsync(id);

            var SalaryDto = SalaryMapper.ToResponseDto(Salary);

            _cacheService.Set(cacheKey, SalaryDto, TimeSpan.FromMinutes(5));

            return ApiResponse<SalaryResponseDto>.Success(SalaryDto,
                                    SalaryDto != null ? AppConstants.Messages.Success
                                        : AppConstants.Messages.NotFound
                                    );
        }

        public async Task<ApiResponse<SalaryResponseDto>> CreateSalaryAsync(SalaryRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newSalary = new Salary
                {
                    EmployeeId = dto.EmployeeId,
                    BasicSalary = dto.BasicSalary,
                    HouseAllowance = dto.HouseAllowance,
                    MedicalAllowance = dto.MedicalAllowance,
                    TransportAllowance = dto.TransportAllowance,
                    EffectiveFrom = dto.EffectiveFrom,
                    EffectiveTo = dto.EffectiveTo,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId
                };

                var Salary = await _unitOfWork.Salaries.CreateSalaryAsync(newSalary);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.SalaryList(1, int.MaxValue, null));

                var createdSalaryDto = SalaryMapper.ToResponseDto(Salary);

                _logger.LogInformation("Salary created by user {UserId}", dto.UserId);

                return ApiResponse<SalaryResponseDto>.Success(createdSalaryDto,
                                        Salary != null ? AppConstants.Messages.Success
                                            : AppConstants.Messages.NotFound
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Salary by user {UserId}", dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<SalaryResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<SalaryResponseDto>> UpdateSalaryAsync(int id, SalaryUpdateRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updateSalaryDto = new Salary
                {
                    EmployeeId = dto.EmployeeId,
                    BasicSalary = dto.BasicSalary,
                    HouseAllowance = dto.HouseAllowance,
                    MedicalAllowance = dto.MedicalAllowance,
                    TransportAllowance = dto.TransportAllowance,
                    EffectiveFrom = dto.EffectiveFrom,
                    EffectiveTo = dto.EffectiveTo,
                    UpdatedBy = dto.UserId
                };

                var Salary = await _unitOfWork.Salaries.UpdateSalaryAsync(id, updateSalaryDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.SalaryList(1, int.MaxValue, null));

                var updatedSalaryDto = SalaryMapper.ToResponseDto(Salary);
                return ApiResponse<SalaryResponseDto>.Success(updatedSalaryDto,
                                        Salary != null ? "Salary updated successfully!"
                                            : "Salary update failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Salary with id {SalaryId} by user {UserId}", id, dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<SalaryResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteSalaryAsync(int id, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isDeleted = await _unitOfWork.Salaries.DeleteSalaryAsync(id, userId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.SalaryList(1, int.MaxValue, null));

                _logger.LogInformation("Salary deleted by user {UserId}", userId);

                return ApiResponse<bool>.Success(isDeleted,
                                        isDeleted ? "Salary deleted successfully!"
                                            : "Salary deletion failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Salary with id {SalaryId} by user {UserId}", id, userId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }
    }
}
