using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestQuestionResponse
    {
       

        public long Id { get; set; }
        public long TestId { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }
    }
}
