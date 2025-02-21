namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestsSubmissionResponse
    {
        public int? TestId { get; set; }
        public int? StudentId { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public int? TotalQuestion { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? WrongAnswers { get; set; }
        public double? Score { get; set; }
        public bool? Graded { get; set; }
    }
}
