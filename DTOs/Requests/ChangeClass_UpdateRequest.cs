namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ChangeClass_UpdateRequest
    {
        public long StudentId { get; set; }
        public long OldClassId { get; set; }
        public DateTime? ChangeClassDate { get; set; }
        public long NewClassId { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public long LeadershipId { get; set; }
        public bool? IsActive { get; set; }

    }
}
