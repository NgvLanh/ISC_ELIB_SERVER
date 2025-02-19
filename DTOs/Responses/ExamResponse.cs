namespace ISC_ELIB_SERVER.Models.Responses
{
    public class ExamResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? DurationMinutes { get; set; }

        // Chuyển enum sang chuỗi khi trả về JSON (nếu cần)
        public string Status { get; set; } = default!;

        public string? File { get; set; }
        public int SemesterId { get; set; }
        public int AcademicYearId { get; set; }
        public int GradeLevelId { get; set; }
        public int ClassTypeId { get; set; }
        public int SubjectId { get; set; }
    }
}
