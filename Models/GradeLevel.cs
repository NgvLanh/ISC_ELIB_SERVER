using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class GradeLevel
    {
        public GradeLevel()
        {
            Classes = new HashSet<Class>();
            ExamSchedules = new HashSet<ExamSchedule>();
            Exams = new HashSet<Exam>();
        }

        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long TeacherId { get; set; }

        public virtual TeacherInfo Teacher { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<ExamSchedule> ExamSchedules { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
