using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class StudentScore
    {
        public long Id { get; set; }
        public double? Score { get; set; }
        public long ScoreTypeId { get; set; }
        public long SubjectId { get; set; }
        public long UserId { get; set; }
        public long SemesterId { get; set; }

        public virtual ScoreType ScoreType { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
