using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Class
    {
        public Class()
        {
            ChangeClassNewClasses = new HashSet<ChangeClass>();
            ChangeClassOldClasses = new HashSet<ChangeClass>();
            ExamScheduleClasses = new HashSet<ExamScheduleClass>();
            Exemptions = new HashSet<Exemption>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? StudentQuantity { get; set; }
        public int? SubjectQuantity { get; set; }
        public string? Description { get; set; }
        public long GradeLevelId { get; set; }
        public long AcademicYearId { get; set; }
        public long? UserId { get; set; }
        public long ClassTypeId { get; set; }

        public virtual AcademicYear AcademicYear { get; set; } = null!;
        public virtual ClassType ClassType { get; set; } = null!;
        public virtual GradeLevel GradeLevel { get; set; } = null!;
        public virtual User? User { get; set; } = null!;
        public virtual ICollection<ChangeClass> ChangeClassNewClasses { get; set; }
        public virtual ICollection<ChangeClass> ChangeClassOldClasses { get; set; }
        public virtual ICollection<ExamScheduleClass> ExamScheduleClasses { get; set; }
        public virtual ICollection<Exemption> Exemptions { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
