namespace HRMS.Domain.Entities
{
    public class EmploymentStatus: BaseEntity
    {
        public string StatusName { get; set; }
        public string StatusDisplayName { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
