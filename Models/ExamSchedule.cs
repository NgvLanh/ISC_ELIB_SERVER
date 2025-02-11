using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ExamSchedule
    {
        public ExamSchedule()
        {
            ExamScheduleClasses = new HashSet<ExamScheduleClass>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDay { get; set; }
        public string? Type { get; set; }
        public bool? Form { get; set; }
        public string? Status { get; set; }
        public long AcademicYearId { get; set; }
        public long Subject { get; set; }
        public long SemesterId { get; set; }
        public long GradeLevelsId { get; set; }

        public virtual AcademicYear AcademicYear { get; set; } = null!;
        public virtual GradeLevel GradeLevels { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual Subject SubjectNavigation { get; set; } = null!;
        public virtual ICollection<ExamScheduleClass> ExamScheduleClasses { get; set; }
    }
}
