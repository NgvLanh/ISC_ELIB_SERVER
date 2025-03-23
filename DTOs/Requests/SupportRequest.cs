using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SupportRequest
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự")]
        public string? Title { get; set; }

        [MaxLength(255, ErrorMessage = "Nội dung không được vượt quá 255 ký tự")]
        public string? Content { get; set; }

        [MaxLength(50, ErrorMessage = "Loại không được vượt quá 100 ký tự")]
        public string? Type { get; set; }

        public DateTime? CreateAt { get; set; }

        [Required(ErrorMessage = "Mã người dùng không được để trống")]
        public int UserId { get; set; }
    }
}
