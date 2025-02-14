namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class QuestionImagesQaResponse
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
