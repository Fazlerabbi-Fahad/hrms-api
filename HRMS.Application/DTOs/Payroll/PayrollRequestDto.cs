namespace HRMS.Application.DTOs.Payroll
{
    public class PayrollRequestDto
    {
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int UserId { get; set; }
    }
}
