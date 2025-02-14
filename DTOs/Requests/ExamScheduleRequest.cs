namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamScheduleRequest
    {
        public string? Name { get; set; }
        public DateTime? ExamDay { get; set; }
        public string? Type { get; set; }
        public bool? Form { get; set; }
        public string? Status { get; set; }
        public long AcademicYearId { get; set; }
        public long Subject { get; set; }
        public long SemesterId { get; set; }
        public long GradeLevelsId { get; set; }
    }
}
