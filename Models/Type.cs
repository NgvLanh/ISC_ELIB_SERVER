using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Type
    {
        public Type()
        {
            Achievements = new HashSet<Achievement>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Achievement> Achievements { get; set; }
    }
}
