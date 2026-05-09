namespace HRMS.Domain.Entities
{
    public class Role:BaseEntity
    {
        public string RoleName { get; set; }
        public string RoleDisplayName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
