using System;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamScheduleRequest
    {
        [Required(ErrorMessage = "Tên lịch thi không được để trống")]
        [StringLength(255, ErrorMessage = "Tên lịch thi không được quá 255 ký tự")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Ngày thi không được để trống")]
        public DateTime? ExamDay { get; set; }

        [Required(ErrorMessage = "Loại bài thi không được để trống")]
        [StringLength(100, ErrorMessage = "Loại bài thi không được quá 100 ký tự")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Hình thức thi không được để trống")]
        public bool? Form { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(50, ErrorMessage = "Trạng thái không được quá 50 ký tự")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Mã năm học không được để trống")]
        public int? AcademicYearId { get; set; }

        [Required(ErrorMessage = "Mã môn học không được để trống")]
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Mã học kỳ không được để trống")]
        public int? SemesterId { get; set; }

        [Required(ErrorMessage = "Mã khối lớp không được để trống")]
        public int? GradeLevelId { get; set; }

        public bool Active { get; set; }
    }
}
