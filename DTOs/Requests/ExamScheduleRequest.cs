using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamScheduleRequest
    {
        public string? Name { get; set; }
        public DateTime? ExamDay { get; set; }
        public string? Type { get; set; }
        public bool? Form { get; set; }
        public ExamScheduleStatus? Status { get; set; }
        public int AcademicYearId { get; set; }
        public int Subject { get; set; }
        public int SemesterId { get; set; }
        public int GradeLevelsId { get; set; }
        public int duration_in_minutes { get; set; }
        public List<int> ClassIds { get; set; } = new List<int>();
        public List<int> GraderIds { get; set; } = new List<int>();
    }
}
