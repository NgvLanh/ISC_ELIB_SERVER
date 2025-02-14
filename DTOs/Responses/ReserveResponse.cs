namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ReserveResponse
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public DateTime ReserveDate { get; set; }
        public string RetentionPeriod { get; set; }
        public string? Reason { get; set; }
        public string? File { get; set; }
        public string Semester { get; set; }
        public long ClassId { get; set; }
        public long SemesterId { get; set; }
        public long LeadershipId { get; set; }
    }
}
