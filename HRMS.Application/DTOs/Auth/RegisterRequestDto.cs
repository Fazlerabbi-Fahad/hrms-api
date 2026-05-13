namespace HRMS.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int? EmployeeId { get; set; }
        public List<int>? RoleIds { get; set; }
    }
}