namespace ISC_ELIB_SERVER.Models.Responses
{
    public class ExamResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? DurationMinutes { get; set; }

        // Chuyển enum sang chuỗi khi trả về JSON (nếu cần)
        public string Status { get; set; } = default!;

        public string? File { get; set; }
        public long SemesterId { get; set; }
        public long AcademicYearId { get; set; }
        public long GradeLevelId { get; set; }
        public long ClassTypeId { get; set; }
        public long SubjectId { get; set; }
    }
}
