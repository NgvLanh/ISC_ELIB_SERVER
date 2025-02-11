using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ClassType
    {
        public ClassType()
        {
            Classes = new HashSet<Class>();
            Exams = new HashSet<Exam>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
