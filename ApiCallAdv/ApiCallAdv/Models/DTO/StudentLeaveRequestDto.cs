namespace ApiCallAdv.Models.DTO
{
    public class StudentLeaveRequestDto
    {
        public Guid LeaveId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty; // ✅ added for teacher view
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
