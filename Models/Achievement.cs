using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Achievement
    {
        public long Id { get; set; }
        public string? Content { get; set; }
        public DateTime? DateAwarded { get; set; }
        public string? File { get; set; }
        public long UserId { get; set; }
        public long TypeId { get; set; }

        public virtual Type Type { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
