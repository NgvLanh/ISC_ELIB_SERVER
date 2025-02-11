using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestSubmissionsAnswer
    {
        public long Id { get; set; }
        public long SubmissionId { get; set; }
        public long QuestionId { get; set; }
        public long SelectedAnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual TestQuestion Question { get; set; } = null!;
        public virtual TestAnswer SelectedAnswer { get; set; } = null!;
        public virtual TestsSubmission Submission { get; set; } = null!;
    }
}
