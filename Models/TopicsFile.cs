using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TopicsFile
    {
        public long Id { get; set; }
        public long TopicId { get; set; }
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Topic Topic { get; set; } = null!;
    }
}
