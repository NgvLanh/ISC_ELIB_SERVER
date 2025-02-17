using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestsAttachment
    {
        public long Id { get; set; }

        [ForeignKey("TestsSubmission")]
        public long SubmissionId { get; set; }
        public string? FileUrl { get; set; }

        public virtual TestsSubmission Submission { get; set; } = null!;
    }
}
