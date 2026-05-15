using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Department;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Application.Mappers;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class DepartmentService:IDepartmentService
    {
        private readonly IDepartmentRepository _DepartmentRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DepartmentService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IDepartmentRepository DepartmentRepository, ICacheService cacheService, ILogger<DepartmentService> logger, IUnitOfWork unitOfWork)
        {
            _DepartmentRepository = DepartmentRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<DepartmentResponseDto>>> GetAllDepartmentsAsync(DepartmentQueryParameters parameters)
        {
            var cacheKey = CacheKeys.DepartmentList(
                    parameters.PageNumber,
                    parameters.PageSize,
                    parameters.Search
                );


            var cached = _cacheService.Get<PagedResult<DepartmentResponseDto>>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Departments retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<PagedResult<DepartmentResponseDto>>.Success(cached,
                                    $"Retrieved Departments successfully!"
                            );
            }

            var (Departments, totalCoount) = await _DepartmentRepository.GetAllDepartmentsAsync(parameters);

            var DepartmentDtos = DepartmentMapper.ToResponseDtoList(Departments);

            var pagedResult = new PagedResult<DepartmentResponseDto>
            {
                Items = DepartmentDtos,
                TotalCount = totalCoount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            _cacheService.Set(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return ApiResponse<PagedResult<DepartmentResponseDto>>.Success(pagedResult, AppConstants.Messages.Success);
        }

        public async Task<ApiResponse<DepartmentResponseDto>> GetDepartmentByIdAsync(int id)
        {
            var cacheKey = CacheKeys.DepartmentById(id);


            var cached = _cacheService.Get<DepartmentResponseDto>(cacheKey);
            if (cached != null)
            {
                _logger.LogInformation("Department retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<DepartmentResponseDto>.Success(cached, AppConstants.Messages.Success);
            }

            var Department = await _DepartmentRepository.GetDepartmentByIdAsync(id);

            var DepartmentDto = DepartmentMapper.ToResponseDto(Department);

            _cacheService.Set(cacheKey, DepartmentDto, TimeSpan.FromMinutes(5));

            return ApiResponse<DepartmentResponseDto>.Success(DepartmentDto,
                                    DepartmentDto != null ? AppConstants.Messages.Success
                                        : AppConstants.Messages.NotFound
                                    );
        }

        public async Task<ApiResponse<DepartmentResponseDto>> CreateDepartmentAsync(DepartmentRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newDepartment = new Department
                {
                    DepartmentName = dto.DepartmentName,
                    DepartmentDisplayName = dto.DepartmentName,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId
                };
                var Department = await _unitOfWork.Departments.CreateDepartmentAsync(newDepartment);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.DepartmentList(1, int.MaxValue, null));

                var createdDepartmentDto = DepartmentMapper.ToResponseDto(Department);

                _logger.LogInformation("Department created by user {UserId}", dto.UserId);

                return ApiResponse<DepartmentResponseDto>.Success(createdDepartmentDto,
                                        Department != null ? AppConstants.Messages.Success
                                            : AppConstants.Messages.NotFound
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating Department by user {UserId}", dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<DepartmentResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<DepartmentResponseDto>> UpdateDepartmentAsync(int id, DepartmentUpdateRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updateDepartmentDto = new Department
                {
                    DepartmentName = dto.DepartmentName,
                    DepartmentDisplayName = dto.DepartmentName,
                    UpdatedBy = dto.UserId
                };

                var Department = await _unitOfWork.Departments.UpdateDepartmentAsync(id, updateDepartmentDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.DepartmentList(1, int.MaxValue, null));

                var updatedDepartmentDto = DepartmentMapper.ToResponseDto(Department);
                return ApiResponse<DepartmentResponseDto>.Success(updatedDepartmentDto,
                                        Department != null ? "Department updated successfully!"
                                            : "Department update failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating Department with id {DepartmentId} by user {UserId}", id, dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<DepartmentResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteDepartmentAsync(int id, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isDeleted = await _unitOfWork.Departments.DeleteDepartmentAsync(id, userId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.DepartmentList(1, int.MaxValue, null));

                _logger.LogInformation("Department deleted by user {UserId}", userId);

                return ApiResponse<bool>.Success(isDeleted,
                                        isDeleted ? "Department deleted successfully!"
                                            : "Department deletion failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting Department with id {DepartmentId} by user {UserId}", id, userId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }
    }
}
