namespace ApiCallAdv.Models.DTO
{
    public class ClassDto
    {
        public Guid ClassId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string ClassTeacherUserId { get; set; } = string.Empty;
        public string? ClassTeacherEmail { get; set; }
        public int StudentCount { get; set; }
    }
}
