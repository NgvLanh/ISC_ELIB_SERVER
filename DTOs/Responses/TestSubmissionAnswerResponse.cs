namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestSubmissionAnswerResponse
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public int QuestionId { get; set; }
        public int SelectedAnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
