public class ExamGraderRequest
{
    public int? ExamId { get; set; }
    public int? UserId { get; set; }
    public string? ClassIds { get; set; }
    public bool Active { get; set; }
}
