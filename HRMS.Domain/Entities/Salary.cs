namespace HRMS.Domain.Entities
{
    public class Salary:BaseEntity
    {
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal HouseAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal? TransportAllowance { get; set; }
        public decimal? Bonus { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public Employee Employee { get; set; } = null!;
        public Payroll Payroll { get; set; } = null!;
    }
}

