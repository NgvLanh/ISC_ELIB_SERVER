using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamGraders = new HashSet<ExamGrader>();
            Sessions = new HashSet<Session>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? DurationMinutes { get; set; }
        public string? Status { get; set; }
        public string? File { get; set; }
        public long SemesterId { get; set; }
        public long AcademicYearId { get; set; }
        public long GradeLevelId { get; set; }
        public long ClassTypeId { get; set; }
        public long SubjectId { get; set; }

        public virtual AcademicYear AcademicYear { get; set; } = null!;
        public virtual ClassType ClassType { get; set; } = null!;
        public virtual GradeLevel GradeLevel { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ICollection<ExamGrader> ExamGraders { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
