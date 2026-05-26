namespace HRMS.Application.DTOs.Menu
{
    public class MenuResponseDto
    {
        public int Id { get; set; }
        public string MenuName { get; set; } = string.Empty;
        public string MenuDisplayName { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string? Icon { get; set; }
        public int Sequence { get; set; }
    }
}
