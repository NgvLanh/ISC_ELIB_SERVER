namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestResponse
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? DurationTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? File { get; set; }
        public string? Description { get; set; }
        public string? ClassIds { get; set; }
        public bool? FileSubmit { get; set; }
        public long? SemesterId { get; set; }
        public long? SubjectId { get; set; }
        public long? UserId { get; set; }
    }
}
