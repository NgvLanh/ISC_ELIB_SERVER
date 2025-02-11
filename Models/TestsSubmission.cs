using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestsSubmission
    {
        public TestsSubmission()
        {
            TestSubmissionsAnswers = new HashSet<TestSubmissionsAnswer>();
            TestsAttachments = new HashSet<TestsAttachment>();
        }

        public long Id { get; set; }
        public long TestId { get; set; }
        public long StudentId { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public int? TotalQuestion { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? WrongAnswers { get; set; }
        public double? Score { get; set; }
        public bool? Graded { get; set; }

        public virtual User Student { get; set; } = null!;
        public virtual Test Test { get; set; } = null!;
        public virtual ICollection<TestSubmissionsAnswer> TestSubmissionsAnswers { get; set; }
        public virtual ICollection<TestsAttachment> TestsAttachments { get; set; }
    }
}
