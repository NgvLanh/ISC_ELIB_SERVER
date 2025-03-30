namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExamScheduleDetailResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDay { get; set; }
        public int DurationInMinutes { get; set; }
        public string? Status { get; set; }
        public string? SemesterName { get; set; }
        public string? GradeLevelName { get; set; }
      
    }
}
