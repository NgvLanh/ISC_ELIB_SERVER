namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class DiscussionImageResponse
    {
        public long Id { get; set; }
        public long DiscussionId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

    }
}
