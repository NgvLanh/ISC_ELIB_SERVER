namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TemporaryLeaveResponse
    {
        public long Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public bool? Status { get; set; }
        public long TeacherId { get; set; }
        public long LeadershipId { get; set; }
        public bool? IsActive { get; set; }

    }
}
