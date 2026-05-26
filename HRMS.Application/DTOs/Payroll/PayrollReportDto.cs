namespace HRMS.Application.DTOs.Payroll
{
    public class PayrollReportDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName => new DateTime(Year, Month, 1)
            .ToString("MMMM");
        public int TotalEmployees { get; set; }
        public decimal TotalGrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal TotalNetSalary { get; set; }
        public int PendingCount { get; set; }
        public int PaidCount { get; set; }
    }
}
