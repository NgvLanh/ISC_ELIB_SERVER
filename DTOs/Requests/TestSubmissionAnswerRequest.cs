using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestSubmissionAnswerRequest
    {
        [Required(ErrorMessage = "Mã số nộp bài không được bỏ trống")]
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "Mã câu hỏi không được bỏ trống")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "Mã câu trả lời đã chọn không được bỏ trống")]
        public int SelectedAnswerId { get; set; }

        public string? AnswerText { get; set; }

        public bool Active { get; set; }
    }
}
