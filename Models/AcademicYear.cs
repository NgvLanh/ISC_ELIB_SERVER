using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class AcademicYear
    {
        public AcademicYear()
        {
            Classes = new HashSet<Class>();
            ExamSchedules = new HashSet<ExamSchedule>();
            Exams = new HashSet<Exam>();
            Semesters = new HashSet<Semester>();
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long SchoolId { get; set; }

        public virtual School School { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<ExamSchedule> ExamSchedules { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<Semester> Semesters { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
