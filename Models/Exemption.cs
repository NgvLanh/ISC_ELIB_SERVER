using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Exemption
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public long ClassId { get; set; }
        public string? ExemptedObjects { get; set; }
        public string? FormExemption { get; set; }
        public bool? IsActive { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual User Student { get; set; } = null!;
    }
}
