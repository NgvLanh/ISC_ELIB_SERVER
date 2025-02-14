using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class WorkProcess
    {
        public long Id { get; set; }
        public long TeacherId { get; set; }
        public string? Organization { get; set; }
        public long SubjectGroupsId { get; set; }
        public string? Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? Active { get; set; }
    }
}
