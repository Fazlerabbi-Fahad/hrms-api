using HRMS.Application.DTOs.Common;

namespace HRMS.Application.DTOs.Employee
{
    public class EmployeeQueryParameters:QueryParameters
    {
        public int ? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? EmploymentStatusId { get; set; }
    }
}
