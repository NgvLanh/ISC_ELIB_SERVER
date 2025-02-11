using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Chat
    {
        public long Id { get; set; }
        public DateTime? SentAt { get; set; }
        public string? Content { get; set; }
        public long UserId { get; set; }
        public long SessionId { get; set; }

        public virtual Session Session { get; set; } = null!;
    }
}
