namespace ApiCallAdv.Models.DTO
{
    public class CreateStudentRequestDto
    {
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public int Marks { get; set; }
        public double AttendancePercentage { get; set; }
        public string? GuardianContact { get; set; }
        public Guid? ClassId { get; set; }
        public string IdentityUserId { get; set; } = string.Empty; // link student to Identity
    }
}
