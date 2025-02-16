namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestsAttachmentRequest
    {
        public int SubmissionId { get; set; }
        public string FileUrl { get; set; } = null!;
    }
}
