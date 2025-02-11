using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Campus
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public long SchoolId { get; set; }
        public long UserId { get; set; }

        public virtual School School { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
