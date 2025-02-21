namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TeachingAssignmentsResponse
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public int? UserId { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? TopicsId { get; set; }
        public bool Active { get; set; }
    }
}
