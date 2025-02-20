using System;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamScheduleRequest
    {
        public string? Name { get; set; }
        public DateTime? ExamDay { get; set; }
        public string? Type { get; set; }
        public bool? Form { get; set; }
        public string? Status { get; set; }
        public int? AcademicYearId { get; set; }
        public int? SubjectId { get; set; }
        public int? SemesterId { get; set; }
        public int? GradeLevelId { get; set; }
        public bool Active { get; set; }
    }
}
