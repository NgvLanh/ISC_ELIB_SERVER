

using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SemesterRequest
    {
        [Required(ErrorMessage = "Thời gian bắt đầu không được để trống")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "Thời gian kết thúc không được để trống")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Niên khóa không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "ID niên khóa không hợp lệ")]
        public int? AcademicYearId { get; set; }
    }
}
