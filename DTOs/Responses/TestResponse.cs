namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? DurationTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? File { get; set; }
        public string? Description { get; set; }
        public string? ClassIds { get; set; }
        public bool? FileSubmit { get; set; }
        public int? SemesterId { get; set; }
        public int? SubjectId { get; set; }
        public int? UserId { get; set; }
    }
}
