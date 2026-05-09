namespace HRMS.Domain.Entities
{
    public class Payroll: BaseEntity
    {
        public int EmployeeId { get; set; }
        public int SalaryId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public int PaymentStatusId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Employee? Employee { get; set; }
        public Salary? Salary { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
