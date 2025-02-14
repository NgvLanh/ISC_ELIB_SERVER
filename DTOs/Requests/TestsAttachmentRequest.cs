namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestsAttachmentRequest
    {
        public long SubmissionId { get; set; }
        public string FileUrl { get; set; } = null!;
    }
}
