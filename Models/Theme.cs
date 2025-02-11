using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Theme
    {
        public Theme()
        {
            SystemSettings = new HashSet<SystemSetting>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<SystemSetting> SystemSettings { get; set; }
    }
}
