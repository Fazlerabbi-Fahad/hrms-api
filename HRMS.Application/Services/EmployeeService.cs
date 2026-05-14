using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;
using System.Reflection.Emit;

namespace HRMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<List<EmployeeResponseDto>>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            var employeeDtos = employees.Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                JoiningDate = e.JoiningDate,
                DepartmentName = e.Department.DepartmentName,
                DesignationName = e.Designation.DesignationDisplayName,
                EmploymentStatusName = e.EmploymentStatus.StatusDisplayName
            }).ToList();

            return ApiResponse<List<EmployeeResponseDto>>.Success(employeeDtos,
                                employeeDtos.Any() ? "Employees retrieved successfully!"
                                    : "No employees found"
                                );
        }

        public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);

            var employeeDto = new EmployeeResponseDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                JoiningDate = employee.JoiningDate,
                DepartmentName = employee.Department.DepartmentName,
                DesignationName = employee.Designation.DesignationDisplayName,
                EmploymentStatusName = employee.EmploymentStatus.StatusDisplayName
            };

            return ApiResponse<EmployeeResponseDto>.Success(employeeDto,
                                    employeeDto != null ? "Employee retrieved successfully!"
                                        : "No employee found"
                                    );
        }

        public async Task<ApiResponse<EmployeeResponseDto>> CreateEmployeeAsync(EmployeeRequestDto dto)
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
            var employee = await _employeeRepository.CreateEmployeeAsync(newEmployee);

            var createdEmployeeDto = new EmployeeResponseDto
            {
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                JoiningDate = employee.JoiningDate,
                DepartmentName = employee.Department.DepartmentName,
                DesignationName = employee.Designation.DesignationDisplayName,
                EmploymentStatusName = employee.EmploymentStatus.StatusDisplayName
            };

            _logger.LogInformation("Employee created by user {UserId}", dto.UserId);

            return ApiResponse<EmployeeResponseDto>.Success(createdEmployeeDto,
                                    employee != null ? "Employee created successfully!"
                                        : "Employee creation failed!"
            );
        }

        public async Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsync(int id, EmployeeUpdateRequestDto dto)
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

            var employee = await _employeeRepository.UpdateEmployeeAsync(id, updateEmployeeDto);

            var updatedEmployeeDto = new EmployeeResponseDto
            {
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                JoiningDate = employee.JoiningDate,
                DepartmentName = employee.Department.DepartmentName,
                DesignationName = employee.Designation.DesignationDisplayName,
                EmploymentStatusName = employee.EmploymentStatus.StatusDisplayName
            };
            return ApiResponse<EmployeeResponseDto>.Success(updatedEmployeeDto,
                                    employee != null ? "Employee updated successfully!"
                                        : "Employee update failed!"
            );
        }

        public async Task<ApiResponse<bool>> DeleteEmployeeAsync(int id, int userId)
        {
            var isDeleted = await _employeeRepository.DeleteEmployeeAsync(id, userId);

            _logger.LogInformation("Employee deleted by user {UserId}", userId);

            return ApiResponse<bool>.Success(isDeleted,
                                    isDeleted ? "Employee deleted successfully!"
                                        : "Employee deletion failed!"
            );
        }
    }
}