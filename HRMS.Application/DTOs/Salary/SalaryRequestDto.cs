namespace HRMS.Application.DTOs.Salary
{
    public class SalaryRequestDto
    {
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal? TransportAllowance { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public int UserId { get; set; }
    }
}
