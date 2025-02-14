using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamGraderRequest
    {
        [Required]
        public long ExamId { get; set; }

        [Required]
        public long UserId { get; set; }

        public string? ClassIds { get; set; }
    }
}
