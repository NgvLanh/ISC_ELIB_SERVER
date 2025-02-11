using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class SubjectGroup
    {
        public SubjectGroup()
        {
            Subjects = new HashSet<Subject>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public long TeacherId { get; set; }

        public virtual TeacherInfo Teacher { get; set; } = null!;
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
