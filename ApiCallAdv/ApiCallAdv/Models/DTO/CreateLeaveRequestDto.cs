namespace ApiCallAdv.Models.DTO
{
    public class CreateLeaveRequestDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
