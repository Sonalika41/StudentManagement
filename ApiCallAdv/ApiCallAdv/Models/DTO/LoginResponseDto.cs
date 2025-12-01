namespace ApiCallAdv.Models.DTO
{
    public class LoginResponseDto
    {
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public string Token { get; set; } = string.Empty;

        // Optional identity-linked claims for client convenience
        public Guid? StudentID { get; set; }
        public Guid? TeacherClassId { get; set; }
    }
}
