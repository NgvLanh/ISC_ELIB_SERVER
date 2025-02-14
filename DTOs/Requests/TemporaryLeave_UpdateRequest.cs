namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TemporaryLeave_UpdateRequest
    {
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public bool? Status { get; set; }
        public int TeacherId { get; set; }
        public int LeadershipId { get; set; }
        public bool Active { get; set; }

    }
}
