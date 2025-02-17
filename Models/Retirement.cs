﻿using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Retirement
    {
        public long Id { get; set; }
        public long TeacherId { get; set; }
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public bool? Status { get; set; }
        public long LeadershipId { get; set; }
        public bool? Active { get; set; }

    }
}
