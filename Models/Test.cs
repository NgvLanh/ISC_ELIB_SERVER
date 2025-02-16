using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Test
    {
        public Test()
        {
            TestFiles = new HashSet<TestFile>();
            TestQuestions = new HashSet<TestQuestion>();
            TestsSubmissions = new HashSet<TestsSubmission>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public int? DurationTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? File { get; set; }
        public string? Description { get; set; }
        public string? ClassIds { get; set; }
        public bool? FileSubmit { get; set; }
        public int? SemesterId { get; set; }
        public int? SubjectId { get; set; }
        public int? UserId { get; set; }
        public bool Active { get; set; }

        public virtual Subject? Subject { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<TestFile> TestFiles { get; set; }
        public virtual ICollection<TestQuestion> TestQuestions { get; set; }
        public virtual ICollection<TestsSubmission> TestsSubmissions { get; set; }
    }
}
