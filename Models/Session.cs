using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Session
    {
        public Session()
        {
            Chats = new HashSet<Chat>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DurationTime { get; set; }
        public string? Password { get; set; }
        public bool? AutoOpen { get; set; }
        public string? ShareCodeUrl { get; set; }
        public string? Status { get; set; }
        public bool? IsExam { get; set; }
        public long TeachingAssignmentId { get; set; }
        public long ExamId { get; set; }

        public virtual Exam Exam { get; set; } = null!;
        public virtual TeachingAssignment TeachingAssignment { get; set; } = null!;
        public virtual ICollection<Chat> Chats { get; set; }
    }
}
