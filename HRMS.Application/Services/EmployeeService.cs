using HRMS.Application.Constants;
using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Application.Mappers;
using HRMS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IEmployeeRepository employeeRepository, ICacheService cacheService, ILogger<EmployeeService> logger, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _cacheService = cacheService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ApiResponse<PagedResult<EmployeeResponseDto>>> GetAllEmployeesAsync(EmployeeQueryParameters parameters)
        {
            var cacheKey = CacheKeys.EmployeeList(
                    parameters.PageNumber,
                    parameters.PageSize,
                    parameters.Search
                );
                

            var cached= _cacheService.Get<PagedResult<EmployeeResponseDto>>(cacheKey);
            if(cached != null)
            {
                _logger.LogInformation("Employees retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<PagedResult<EmployeeResponseDto>>.Success(cached,
                                    $"Retrieved employees successfully!"
                            );
            }

            var (employees, totalCoount) = await _employeeRepository.GetAllEmployeesAsync(parameters);

            var employeeDtos = EmployeeMapper.ToResponseDtoList(employees);

            var pagedResult=new PagedResult<EmployeeResponseDto>
                                    {
                                        Items= employeeDtos,
                                        TotalCount = totalCoount,
                                        PageNumber = parameters.PageNumber,
                                        PageSize = parameters.PageSize
                                    };
            
            _cacheService.Set(cacheKey, pagedResult, TimeSpan.FromMinutes(5));

            return ApiResponse<PagedResult<EmployeeResponseDto>>.Success(pagedResult,AppConstants.Messages.Success);
        }

        public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(int id)
        {
            var cacheKey = CacheKeys.EmployeeById(id);


            var cached = _cacheService.Get<EmployeeResponseDto>(cacheKey);
            if(cached != null)
            {
                _logger.LogInformation("Employee retrieved from cache with key {CacheKey}", cacheKey);
                return ApiResponse<EmployeeResponseDto>.Success(cached, AppConstants.Messages.Success);
            }

            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            var employeeDto = EmployeeMapper.ToResponseDto(employee);

            _cacheService.Set(cacheKey, employeeDto, TimeSpan.FromMinutes(5));

            return ApiResponse<EmployeeResponseDto>.Success(employeeDto,
                                    employeeDto != null ? AppConstants.Messages.Success
                                        : AppConstants.Messages.NotFound
                                    );
        }

        public async Task<ApiResponse<EmployeeResponseDto>> CreateEmployeeAsync(EmployeeRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var newEmployee = new Employee
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    JoiningDate = dto.JoiningDate,
                    DateOfBirth = dto.DateOfBirth,
                    DepartmentId = dto.DepartmentId,
                    DesignationId = dto.DesignationId,
                    EmploymentStatusId = dto.EmploymentStatusId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId
                };
                var employee = await _unitOfWork.Employees.CreateEmployeeAsync(newEmployee);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.EmployeeList(1, int.MaxValue, null));

                var createdEmployeeDto = EmployeeMapper.ToResponseDto(employee);

                _logger.LogInformation("Employee created by user {UserId}", dto.UserId);

                return ApiResponse<EmployeeResponseDto>.Success(createdEmployeeDto,
                                        employee != null ? AppConstants.Messages.Success
                                            : AppConstants.Messages.NotFound
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating employee by user {UserId}", dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<EmployeeResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsync(int id, EmployeeUpdateRequestDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var updateEmployeeDto = new Employee
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    DateOfBirth = dto.DateOfBirth,
                    DepartmentId = dto.DepartmentId,
                    DesignationId = dto.DesignationId,
                    EmploymentStatusId = dto.EmploymentStatusId,
                    UpdatedBy = dto.UserId
                };

                var employee = await _unitOfWork.Employees.UpdateEmployeeAsync(id, updateEmployeeDto);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.EmployeeList(1, int.MaxValue, null));

                var updatedEmployeeDto = EmployeeMapper.ToResponseDto(employee);
                return ApiResponse<EmployeeResponseDto>.Success(updatedEmployeeDto,
                                        employee != null ? "Employee updated successfully!"
                                            : "Employee update failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating employee with id {EmployeeId} by user {UserId}", id, dto.UserId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<EmployeeResponseDto>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }

        public async Task<ApiResponse<bool>> DeleteEmployeeAsync(int id, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var isDeleted = await _unitOfWork.Employees.DeleteEmployeeAsync(id, userId);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                _cacheService.Remove(CacheKeys.EmployeeList(1, int.MaxValue, null));

                _logger.LogInformation("Employee deleted by user {UserId}", userId);

                return ApiResponse<bool>.Success(isDeleted,
                                        isDeleted ? "Employee deleted successfully!"
                                            : "Employee deletion failed!"
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting employee with id {EmployeeId} by user {UserId}", id, userId);
                await _unitOfWork.RollbackTransactionAsync();
                return ApiResponse<bool>.Failure(new List<string> { AppConstants.Messages.ServerError },
                                        AppConstants.Messages.ServerError,
                                        500
                );
            }
        }
    }
}