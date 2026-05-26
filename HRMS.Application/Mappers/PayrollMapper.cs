using HRMS.Application.DTOs.Payroll;
using HRMS.Domain.Entities;

namespace HRMS.Application.Mappers
{
    public static class PayrollMapper
    {
        public static PayrollResponseDto ToResponseDto(Payroll payroll)
        {
            return new PayrollResponseDto
            {
                Id = payroll.Id,
                EmployeeId = payroll.EmployeeId,
                EmpCode = payroll.Employee?.EmpCode ?? string.Empty,
                EmployeeName = payroll.Employee?.Name ?? string.Empty,
                Department = payroll.Employee?.Department?.DepartmentName
                    ?? string.Empty,
                Month = payroll.Month,
                Year = payroll.Year,
                BasicSalary = payroll.Salary?.BasicSalary ?? 0,
                HouseAllowance = payroll.Salary?.HouseAllowance ?? 0,
                MedicalAllowance = payroll.Salary?.MedicalAllowance ?? 0,
                TransportAllowance = payroll.Salary?.TransportAllowance ?? 0,
                Bonus = payroll.Salary?.Bonus ?? 0,
                GrossSalary = payroll.GrossSalary,
                TotalDeductions = payroll.TotalDeductions,
                NetSalary = payroll.NetSalary,
                PaymentStatus = payroll.PaymentStatus?.StatusName ?? string.Empty,
                PaymentDate = payroll.PaymentDate,
                CreatedAt = payroll.CreatedAt
            };
        }
    }
}
