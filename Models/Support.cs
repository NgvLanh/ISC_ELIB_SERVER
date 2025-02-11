﻿using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Support
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public DateTime? CreateAt { get; set; }
        public long UserId { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
