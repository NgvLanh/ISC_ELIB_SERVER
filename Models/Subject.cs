using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Subject
    {
        public Subject()
        {
            ExamSchedules = new HashSet<ExamSchedule>();
            Exams = new HashSet<Exam>();
            QuestionQas = new HashSet<QuestionQa>();
            StudentScores = new HashSet<StudentScore>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
            Tests = new HashSet<Test>();
        }

        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? HoursSemester1 { get; set; }
        public int? HoursSemester2 { get; set; }
        public long SubjectGroupId { get; set; }
        public long SubjectTypeId { get; set; }

        public virtual SubjectGroup SubjectGroup { get; set; } = null!;
        public virtual SubjectType SubjectType { get; set; } = null!;
        public virtual ICollection<ExamSchedule> ExamSchedules { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<QuestionQa> QuestionQas { get; set; }
        public virtual ICollection<StudentScore> StudentScores { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
