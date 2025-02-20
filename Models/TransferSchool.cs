using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TransferSchool
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public DateTime? TransferSchoolDate { get; set; }
        public string? TransferToSchool { get; set; }
        public string? SchoolAddress { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public int? LeadershipId { get; set; }
        public bool Active { get; set; }

        public virtual User? Student { get; set; }
    }
}
