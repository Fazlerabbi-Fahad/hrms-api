namespace HRMS.Domain.Entities
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public DateTime JoiningDate { get; set; }
        public int EmploymentStatusId { get; set; }

        public Department Department { get; set; } = null!;
        public Designation Designation { get; set; } = null!;
        public EmploymentStatus EmploymentStatus { get; set; }=null!;
        public User User { get; set; }=null!;
        public Salary Salary { get; set; } = new Salary();
        public Payroll Payroll { get; set; }=new Payroll();

    }
}
