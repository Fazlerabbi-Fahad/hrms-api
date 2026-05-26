namespace HRMS.Application.DTOs.Payroll
{
    public class PayrollResponseDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmpCode { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName => new DateTime(Year, Month, 1)
            .ToString("MMMM"); 
        public decimal BasicSalary { get; set; }
        public decimal HouseAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal TransportAllowance { get; set; }
        public decimal Bonus { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
