using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TemporaryLeave
    {
        public long Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public bool? Status { get; set; }
        public long TeacherId { get; set; }
        public long LeadershipId { get; set; }
        public bool? IsActive { get; set; }

        public virtual TeacherInfo Teacher { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
