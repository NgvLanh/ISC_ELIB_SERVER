using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ExamGrader
    {
        public long Id { get; set; }
        public long ExamId { get; set; }
        public long UserId { get; set; }
        public string? ClassIds { get; set; }

        public virtual Exam Exam { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
