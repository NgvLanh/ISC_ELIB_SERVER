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

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TeacherId { get; set; }
        public bool Active { get; set; }

        public virtual TeacherInfo? Teacher { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
