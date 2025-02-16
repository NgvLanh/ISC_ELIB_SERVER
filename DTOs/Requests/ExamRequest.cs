using System.ComponentModel.DataAnnotations;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamRequest
    {

        [Required(ErrorMessage = "Tên bài kiểm tra không được để trống")]
        [StringLength(255, ErrorMessage = "Tên bài kiểm tra không được quá 255 ký tự")]
        public string Name { get; set; }

        [Range(1, 300, ErrorMessage = "Thời gian làm bài phải từ 1 đến 300 phút")]
        public int? DurationMinutes { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public ExamStatus Status { get; set; }

        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "SemesterId không được để trống")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage = "AcademicYearId không được để trống")]
        public int AcademicYearId { get; set; }

        [Required(ErrorMessage = "GradeLevelId không được để trống")]
        public int GradeLevelId { get; set; }

        [Required(ErrorMessage = "ClassTypeId không được để trống")]
        public int ClassTypeId { get; set; }

        [Required(ErrorMessage = "SubjectId không được để trống")]
        public int SubjectId { get; set; }
    }

    public enum ExamStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
