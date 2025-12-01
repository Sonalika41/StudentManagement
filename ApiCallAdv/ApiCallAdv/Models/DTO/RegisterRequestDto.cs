namespace ApiCallAdv.Models.DTO
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Student|Teacher|Principal (IT is seeded only)
        public string? FullName { get; set; }
    }
}
