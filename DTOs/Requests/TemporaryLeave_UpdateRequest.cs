namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TemporaryLeave_UpdateRequest
    {
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public bool? Status { get; set; }
        public long TeacherId { get; set; }
        public long LeadershipId { get; set; }
        public bool? IsActive { get; set; }

    }
}
