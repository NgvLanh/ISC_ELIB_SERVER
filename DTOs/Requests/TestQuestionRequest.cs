using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestQuestionRequest
    {

        public long? TestId { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }

       
    }
}
