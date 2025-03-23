using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    
    public partial class TestQuestion
    {    public enum QuestionTypeEnum
        {
            TracNghiem = 0,
            TuLuan = 1
        }
        public TestQuestion()
        {
            TestAnswers = new HashSet<TestAnswer>();
            TestSubmissionsAnswers = new HashSet<TestSubmissionsAnswer>();
        }

        public int Id { get; set; }
        public int? TestId { get; set; }
        public string? QuestionText { get; set; }
        [Column("question_type")]
        public QuestionType QuestionType { get; set; }
        public bool Active { get; set; }

        public virtual Test? Test { get; set; }
        public virtual ICollection<TestAnswer> TestAnswers { get; set; }
        public virtual ICollection<TestSubmissionsAnswer> TestSubmissionsAnswers { get; set; }
    }
}
