using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class SubjectType
    {
        public SubjectType()
        {
            Subjects = new HashSet<Subject>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}
