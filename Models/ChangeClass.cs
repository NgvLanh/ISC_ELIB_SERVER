using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ChangeClass
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public long OldClassId { get; set; }
        public DateTime? ChangeClassDate { get; set; }
        public long NewClassId { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public long LeadershipId { get; set; }

        public virtual Class NewClass { get; set; } = null!;
        public virtual Class OldClass { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
    }
}
