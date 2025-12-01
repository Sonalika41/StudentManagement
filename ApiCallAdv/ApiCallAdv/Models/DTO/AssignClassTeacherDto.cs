namespace ApiCallAdv.Models.DTO
{
    public class AssignClassTeacherDto
    {
        public Guid ClassId { get; set; }
        public string TeacherUserId { get; set; } = string.Empty;
    }
}
