namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TransferSchoolResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime? TransferSchoolDate { get; set; }
        public string? TransferToSchool { get; set; }
        public string? SchoolAddress { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public int LeadershipId { get; set; }

    }
}
