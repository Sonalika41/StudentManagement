namespace ApiCallAdv.Models.DTO
{
    public class UpdateLeaveStatusDto
    {
        public Guid LeaveId { get; set; }
        public string Status { get; set; } = string.Empty;// Approved|Rejected
    }
}
