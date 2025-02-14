namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class QuestionQaResponse
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public long UserId { get; set; }
        public long SubjectId { get; set; }
    }
}
