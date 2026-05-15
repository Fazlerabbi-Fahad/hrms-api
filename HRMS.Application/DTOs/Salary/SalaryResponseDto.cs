namespace HRMS.Application.DTOs.Salary
{
    public class SalaryResponseDto
    {
        public int Id { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal? TransportAllowance { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
    }
}
