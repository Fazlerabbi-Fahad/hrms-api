using HRMS.Application.DTOs.Common;

namespace HRMS.Application.DTOs.PaymentStatus
{
    public class PaymentStatusQueryParameters:QueryParameters
    {
        public string? StatusName { get; set; }
    }
}
