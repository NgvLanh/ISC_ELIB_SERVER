using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestSubmissionAnswerRequest
    {
        [Required(ErrorMessage = "Mã số nộp bài không được bỏ trống")]
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "Mã câu hỏi không được bỏ trống")]
        public int QuestionId { get; set; }

        // Optional vì có thể là tự luận
        public int? SelectedAnswerId { get; set; }

        public string? AnswerText { get; set; }

        public bool Active { get; set; } = true;

        public double? Score { get; set; }

        public string? TeacherComment { get; set; }

        // Danh sách file đính kèm
        //public List<TestSubmissionAnswerAttachmentRequest>? Attachments { get; set; }
    }
}
