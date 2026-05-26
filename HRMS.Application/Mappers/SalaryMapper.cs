using HRMS.Application.DTOs.Salary;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public class SalaryMapper
    {
        public static SalaryResponseDto ToResponseDto(Salary Salary)
        {
            return new SalaryResponseDto
            {
                Id = Salary.Id,
                EmployeeName = Salary.Employee?.Name ?? string.Empty,
                BasicSalary = Salary.BasicSalary,
                HouseAllowance = Salary.HouseAllowance,
                MedicalAllowance = Salary.MedicalAllowance,
                TransportAllowance = Salary.TransportAllowance,
                EffectiveFrom = Salary.EffectiveFrom,
                EffectiveTo = Salary.EffectiveTo
            };
        }
        public static List<SalaryResponseDto> ToResponseDtoList(List<Salary> Salarys)
        {
            return Salarys.Select(ToResponseDto).ToList();
        }

        public static Salary ToEntity(SalaryRequestDto requestDto)
        {
            return new Salary
            {
                EmployeeId=requestDto.EmployeeId,
                BasicSalary=requestDto.BasicSalary,
                HouseAllowance=requestDto.HouseAllowance,
                MedicalAllowance=requestDto.MedicalAllowance,
                TransportAllowance=requestDto.TransportAllowance,
                EffectiveFrom=requestDto.EffectiveFrom,
                EffectiveTo=requestDto.EffectiveTo,

            };
        }
    }
}