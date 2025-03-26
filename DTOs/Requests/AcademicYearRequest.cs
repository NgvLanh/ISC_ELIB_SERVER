using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class AcademicYearRequest
    {
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Date)]

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Trường học không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "ID trường học không hợp lệ")]
        public int? SchoolId { get; set; }
    }

    public class AcademicYearSemesterRequest
    {

        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

}
