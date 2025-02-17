namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExamGraderResponse
    {
        public long Id { get; set; }
        public long ExamId { get; set; }
        public long UserId { get; set; }
        public string? ClassIds { get; set; }
    }
}
