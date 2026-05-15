using HRMS.Application.DTOs.Designation;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public class DesignationMapper
    {
        public static DesignationResponseDto ToResponseDto(Designation Designation)
        {
            return new DesignationResponseDto
            {
                Id = Designation.Id,
                DesignationName = Designation.DesignationName,
            };
        }
        public static List<DesignationResponseDto> ToResponseDtoList(List<Designation> Designations)
        {
            return Designations.Select(ToResponseDto).ToList();
        }
    }
}
