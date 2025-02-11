using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeachingAssignment
    {
        public TeachingAssignment()
        {
            Sessions = new HashSet<Session>();
        }

        public long Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
        public long UserId { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public long TopicsId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual Topic Topics { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
