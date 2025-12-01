namespace ApiCallAdv.Models.Domain
{
    public class Class
    {
        public Guid ClassId { get; set; }
        public string Name { get; set; } = string.Empty;    // e.g., "Grade 8"
        public string Section { get; set; } = string.Empty; // e.g., "A"

        // Link to Identity user (Teacher account)
        public string ClassTeacherUserId { get; set; } = string.Empty;

        public ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
