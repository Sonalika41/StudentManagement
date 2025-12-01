namespace ApiCallAdv.Models.DTO
{
    public class StudentDto
    {
        public Guid StudentID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Course { get; set; } = string.Empty;
        public DateTime EnrollmentDate { get; set; }
        public int Marks { get; set; }
        public double AttendancePercentage { get; set; }
        public string? GuardianContact { get; set; }
        public Guid? ClassId { get; set; }
        public string? ClassName { get; set; }
        public string? ClassSection { get; set; }
        public string? ClassTeacherEmail { get; set; }
    }
}
