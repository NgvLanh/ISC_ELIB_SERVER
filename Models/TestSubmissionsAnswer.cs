using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestSubmissionsAnswer
    {
        public int Id { get; set; }
        public int? SubmissionId { get; set; }
        public int? QuestionId { get; set; }
        public int? SelectedAnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool? IsCorrect { get; set; }
        public bool Active { get; set; }

        public virtual TestQuestion? Question { get; set; }
        public virtual TestAnswer? SelectedAnswer { get; set; }
        public virtual TestsSubmission? Submission { get; set; }
    }
}
