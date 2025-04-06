using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class EducationLevelRequest
    {
        [Required(ErrorMessage = "Tên bậc học - trình độ không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên bậc học - trình độ không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        public bool? Status { get; set; }

        [MaxLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string? Description { get; set; }


    }
}
