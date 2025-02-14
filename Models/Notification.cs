using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Notification
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public DateTime? CreateAt { get; set; }
        public long SenderId { get; set; }
        public long UserId { get; set; }

        public virtual User? Sender { get; set; }
        public virtual User? User { get; set; }
    }
}
