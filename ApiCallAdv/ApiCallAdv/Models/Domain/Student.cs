namespace ApiCallAdv.Models.Domain
{
    public class Student
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

        // Link to Identity user (Student account)
        public string IdentityUserId { get; set; } = string.Empty;

        // Class mapping
        public Guid? ClassId { get; set; }
        public Class? Class { get; set; }
    }
}
