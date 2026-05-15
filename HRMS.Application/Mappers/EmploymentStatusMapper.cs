using HRMS.Application.DTOs.EmploymentStatus;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public class EmploymentStatusMapper
    {
        public static EmploymentStatusResponseDto ToResponseDto(EmploymentStatus EmploymentStatus)
        {
            return new EmploymentStatusResponseDto
            {
                Id = EmploymentStatus.Id,
                StatusName = EmploymentStatus.StatusName,
            };
        }
        public static List<EmploymentStatusResponseDto> ToResponseDtoList(List<EmploymentStatus> EmploymentStatuss)
        {
            return EmploymentStatuss.Select(ToResponseDto).ToList();
        }
    }
}
