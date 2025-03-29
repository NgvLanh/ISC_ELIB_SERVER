namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class AchivementRequest
    {
        public string? Content { get; set; }
        public DateTime DateAwarded { get; set; }
        public string? File { get; set; }
        public int? UserId { get; set; }
        public int? TypeId { get; set; }
    }
}
