using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ReserveRequest
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        public long StudentId { get; set; }

        [Required(ErrorMessage = "Ngày đặt không được để trống")]
        public DateTime ReserveDate { get; set; }

        [MaxLength(50, ErrorMessage = "Thời gian giữ chỗ không được vượt quá 50 ký tự")]
        public string RetentionPeriod { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Lý do không được vượt quá 200 ký tự")]
        public string? Reason { get; set; }

        [MaxLength(255, ErrorMessage = "Tên tệp không được vượt quá 255 ký tự")]
        public string? File { get; set; }

        [Required(ErrorMessage = "Học kỳ không được để trống")]
        [MaxLength(50, ErrorMessage = "Học kỳ không được vượt quá 50 ký tự")]
        public string Semester { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public long ClassId { get; set; }

        [Required(ErrorMessage = "Mã học kỳ không được để trống")]
        public long SemesterId { get; set; } // Corrected property name

        [Required(ErrorMessage = "Mã lãnh đạo không được để trống")]
        public long LeadershipId { get; set; }
    }
}
