using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestRequest
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Loại không được để trống.")]
        [MaxLength(50, ErrorMessage = "Loại không được vượt quá 50 ký tự.")]
        public string? Type { get; set; }

        [Range(1, 360, ErrorMessage = "Thời gian phải trong khoảng 1 đến 360 phút.")]
        public int? DurationTime { get; set; }

        [Required(ErrorMessage = "Thời gian bắt đầu không được để trống.")]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessage = "Thời gian kết thúc không được để trống.")]
        //[DateGreaterThan("StartTime", ErrorMessage = "Thời gian kết thúc phải sau thời gian bắt đầu.")]
        public DateTime? EndTime { get; set; }

        public string? File { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Phải có ít nhất một lớp tham gia.")]
        public string? ClassIds { get; set; }

        public bool? FileSubmit { get; set; }

        //[Required(ErrorMessage = "Không được để trống.")]
        public long? SemesterId { get; set; }

        //[Required(ErrorMessage = "Không được để trống.")]
        public long? SubjectId { get; set; }

        //[Required(ErrorMessage = "Không được để trống.")]
        public long? UserId { get; set; }
    }
}
