namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TransferSchoolResponse
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public DateTime? TransferSchoolDate { get; set; }
        public string? TransferToSchool { get; set; }
        public string? SchoolAddress { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public long LeadershipId { get; set; }
        public bool? IsActive { get; set; }

    }
}
