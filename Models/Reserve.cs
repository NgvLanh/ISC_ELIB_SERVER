using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Reserve
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public DateTime? ReserveDate { get; set; }
        public string? RetentionPeriod { get; set; }
        public string? Reason { get; set; }
        public string? File { get; set; }
        public string? Semester { get; set; }
        public long ClassId { get; set; }
        public long SemestersId { get; set; }
        public long LeadershipId { get; set; }

        public virtual User? Student { get; set; }
    }
}
