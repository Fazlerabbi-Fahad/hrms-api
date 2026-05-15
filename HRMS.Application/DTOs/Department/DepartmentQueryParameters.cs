using HRMS.Application.DTOs.Common;

namespace HRMS.Application.DTOs.Department
{
    public class DepartmentQueryParameters:QueryParameters
    {
        public string? DepartmentName { get; set; }
    }
}
