using HRMS.Application.DTOs.Common;

namespace HRMS.Application.DTOs.Salary
{
    public class SalaryQueryParameters:QueryParameters
    {
        public int? EmployeeId { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
    }
}
