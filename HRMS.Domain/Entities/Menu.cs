namespace HRMS.Domain.Entities
{
    public class Menu:BaseEntity
    {
        public string MenuName { get; set; }
        public string MenuDisplayName { get; set; }
        public int Sequence { get; set; }
        public string Route { get; set; }
        public string? Icon { get; set; }
    }
}
