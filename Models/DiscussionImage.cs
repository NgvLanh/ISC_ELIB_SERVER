using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class DiscussionImage
    {
        public long Id { get; set; }
        public long DiscussionId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual Discussion Discussion { get; set; } = null!;
    }
}
