namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SupportResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public DateTime? CreateAt { get; set; }
        public int UserId { get; set; }
    }
}
