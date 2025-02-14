using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestSubmissionAnswerRequest
    {
        [Required(ErrorMessage = "SubmissionId is required.")]
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "QuestionId is required.")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "SelectedAnswerId is required.")]
        public int SelectedAnswerId { get; set; }

        public string? AnswerText { get; set; }
    }
}
