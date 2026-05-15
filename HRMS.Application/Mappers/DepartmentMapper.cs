using HRMS.Application.DTOs.Department;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public class DepartmentMapper
    {
        public static DepartmentResponseDto ToResponseDto(Department Department)
        {
            return new DepartmentResponseDto
            {
                Id = Department.Id,
                DepartmentName = Department.DepartmentName,
            };
        }
        public static List<DepartmentResponseDto> ToResponseDtoList(List<Department> Departments)
        {
            return Departments.Select(ToResponseDto).ToList();
        }

    }
}
