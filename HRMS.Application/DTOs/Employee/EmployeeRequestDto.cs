namespace HRMS.Application.DTOs.Employee
{
    public class EmployeeRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoiningDate { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int EmploymentStatusId { get; set; }
        public int UserId { get; set; }
    }
}
