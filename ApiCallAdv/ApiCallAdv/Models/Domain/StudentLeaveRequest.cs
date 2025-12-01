namespace ApiCallAdv.Models.Domain
{
    public class StudentLeaveRequest
    {
        public Guid LeaveId { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Pending|Approved|Rejected
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
