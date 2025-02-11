using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Discussion
    {
        public Discussion()
        {
            DiscussionImages = new HashSet<DiscussionImage>();
        }

        public long Id { get; set; }
        public long TopicId { get; set; }
        public long UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Topic Topic { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<DiscussionImage> DiscussionImages { get; set; }
    }
}
