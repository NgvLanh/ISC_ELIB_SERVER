namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class AnswerImagesQaResponse
    {
        public long Id { get; set; }
        public long AnswerId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
