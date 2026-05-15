using HRMS.Application.DTOs.Role;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public class RoleMapper
    {
        public static RoleResponseDto ToResponseDto(Role Role)
        {
            return new RoleResponseDto
            {
                Id = Role.Id,
                RoleName = Role.RoleName,
            };
        }
        public static List<RoleResponseDto> ToResponseDtoList(List<Role> Roles)
        {
            return Roles.Select(ToResponseDto).ToList();
        }
    }
}
