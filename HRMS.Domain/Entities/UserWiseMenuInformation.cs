namespace HRMS.Domain.Entities
{
    public class UserWiseMenuInformation:BaseEntity
    {
        public int UserId { get; set; }
        public int MenuId { get; set; }
    }
}
