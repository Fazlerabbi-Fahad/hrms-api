namespace HRMS.Domain.Entities
{
    public class Designation: BaseEntity
    {
        public string DesignationName { get; set; }
        public string DesignationDisplayName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
