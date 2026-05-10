using HRMS.Application.DTOs.Common;
using HRMS.Application.DTOs.Employee;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repository;
using HRMS.Domain.Entities;

namespace HRMS.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<ApiResponse<List<EmployeeResponseDto>>> GetAllEmployeesAsync()
        {
            try
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
                                    employeeDtos.Any() ?"Employees retrieved successfully!"
                                        :"No employees found"
                                    );
            }
            catch (Exception ex)
            {
                return ApiResponse<List<EmployeeResponseDto>>.Failure(null,ex.Message, 500);
            }
        }

        public async Task<ApiResponse<EmployeeResponseDto>> GetEmployeeByIdAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                return ApiResponse<EmployeeResponseDto>.Failure(null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<EmployeeResponseDto>> CreateEmployeeAsync(EmployeeRequestDto dto)
        {
            try
            {
                var employee = await _employeeRepository.CreateEmployeeAsync(dto);

                return ApiResponse<EmployeeResponseDto>.Success(null,
                                        employee != null ? "Employee created successfully!"
                                            : "Employee creation failed!"
                );
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponse<EmployeeResponseDto>.Failure(null, ex.Message, 409);
            }
            catch (Exception ex)
            {
                return ApiResponse<EmployeeResponseDto>.Failure(null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<EmployeeResponseDto>> UpdateEmployeeAsync(int id, EmployeeUpdateRequestDto dto)
        {
            try
            {
                var employee = await _employeeRepository.UpdateEmployeeAsync(id, dto);
                return ApiResponse<EmployeeResponseDto>.Success(null,
                                      employee != null ? "Employee updated successfully!"
                                          : "Employee update failed!"
              );
            }
            catch (Exception ex)
            {
                return ApiResponse<EmployeeResponseDto>.Failure(null, ex.Message, 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteEmployeeAsync(int id,int userId)
        {
            try
            {
                var isDeleted = await _employeeRepository.DeleteEmployeeAsync(id, userId);
                return ApiResponse<bool>.Success(isDeleted,
                                     isDeleted ? "Employee deleted successfully!"
                                         : "Employee deletion failed!"
                );
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure(null, ex.Message, 500);
            }
        }
    }
}
