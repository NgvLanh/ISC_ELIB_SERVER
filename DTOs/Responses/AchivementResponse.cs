using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class AchivementResponse
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string? File { get; set; }
        public int? UserId { get; set; }
        public int? TypeId { get; set; }
        public string? TypeValue  { get; set; }
    }
}
