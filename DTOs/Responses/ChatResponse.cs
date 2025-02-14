namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ChatResponse
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public long UserId { get; set; }
        public long SessionId { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
