using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamGraderRequest
    {
        [Required(ErrorMessage = "ExamId không được để trống.")]
        public long ExamId { get; set; }

        [Required(ErrorMessage = "UserId không được để trống.")]
        public long UserId { get; set; }

        public string? ClassIds { get; set; }
    }
}
