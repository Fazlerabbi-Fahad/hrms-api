using HRMS.Application.DTOs.Employee;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public class EmployeeMapper
    {
        public static EmployeeResponseDto ToResponseDto(Employee employee)
        {
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
                JoiningDate = employee.JoiningDate,
                DepartmentName = employee.Department.DepartmentName,
                DesignationName = employee.Designation.DesignationName,
                EmploymentStatusName = employee.EmploymentStatus.StatusName
            };
        }
        public static List<EmployeeResponseDto> ToResponseDtoList(List<Employee> employees)
        {
            return employees.Select(ToResponseDto).ToList();
        }

        public static Employee ToEntity(EmployeeRequestDto requestDto)
        {
            return new Employee
            {
                Name = requestDto.Name,
                Email = requestDto.Email,
                PhoneNumber = requestDto.PhoneNumber,
                DateOfBirth = requestDto.DateOfBirth,
                JoiningDate = requestDto.JoiningDate,
                DepartmentId = requestDto.DepartmentId,
                DesignationId = requestDto.DesignationId,
                EmploymentStatusId = requestDto.EmploymentStatusId,
                CreatedBy = requestDto.UserId
            };
        }
    }
}
