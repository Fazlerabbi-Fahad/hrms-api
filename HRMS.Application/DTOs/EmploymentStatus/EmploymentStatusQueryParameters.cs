using HRMS.Application.DTOs.Common;

namespace HRMS.Application.DTOs.EmploymentStatus
{
    public class EmploymentStatusQueryParameters:QueryParameters
    {
        public string? StatusName { get; set; }
    }
}
