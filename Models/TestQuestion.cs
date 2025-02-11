using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestQuestion
    {
        public TestQuestion()
        {
            TestAnswers = new HashSet<TestAnswer>();
            TestSubmissionsAnswers = new HashSet<TestSubmissionsAnswer>();
        }

        public long Id { get; set; }
        public long TestId { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }

        public virtual Test Test { get; set; } = null!;
        public virtual ICollection<TestAnswer> TestAnswers { get; set; }
        public virtual ICollection<TestSubmissionsAnswer> TestSubmissionsAnswers { get; set; }
    }
}
