using HRMS.Application.DTOs.Common;

namespace HRMS.Application.DTOs.Payroll
{
    public class PayrollQueryParameters : QueryParameters
    {
        public int? EmployeeId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public int? PaymentStatusId { get; set; }
    }
}
