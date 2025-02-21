using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamGraderRequest
    {
        [Required(ErrorMessage = "Mã bài kiểm tra không được để trống")]
        public int? ExamId { get; set; }

        [Required(ErrorMessage = "Mã người dùng không được để trống")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Danh sách mã lớp không được để trống")]
        [StringLength(500, ErrorMessage = "Danh sách mã lớp không được vượt quá 500 ký tự")]
        public string? ClassIds { get; set; }

        public bool Active { get; set; }
    }
}
