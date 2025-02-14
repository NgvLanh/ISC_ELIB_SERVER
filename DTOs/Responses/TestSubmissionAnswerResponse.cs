namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestSubmissionAnswerResponse
    {
        public long Id { get; set; }
        public long SubmissionId { get; set; }
        public long QuestionId { get; set; }
        public long SelectedAnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
