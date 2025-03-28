using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.Requests
{
    public class ExamScheduleClassRequest
    {
        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int? ClassId { get; set; }

        [Required(ErrorMessage = "Mã lịch thi không được để trống")]
        public int? ExamSchedule { get; set; }  // Sửa tên từ ExampleSchedule -> ExamSchedule (giả định là lỗi đánh máy)

        [Required(ErrorMessage = "Mã giáo viên giám sát không được để trống")]
        public int? SupervisoryTeacherId { get; set; }

        public bool Active { get; set; }
    }
}
