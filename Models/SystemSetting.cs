using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class SystemSetting
    {
        public long Id { get; set; }
        public bool? Captcha { get; set; }
        public long UserId { get; set; }
        public long ThemeId { get; set; }

        public virtual Theme Theme { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
