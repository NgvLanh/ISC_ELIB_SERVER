﻿using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Reserve
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public DateTime? ReserveDate { get; set; }
        public string? RetentionPeriod { get; set; }
        public string? Reason { get; set; }
        public string? File { get; set; }
        public string? Semester { get; set; }
        public int? ClassId { get; set; }
        public int? SemestersId { get; set; }
        public int? UserId { get; set; }
        public bool Active { get; set; }

        public virtual User? Student { get; set; }
        public virtual Class? Class { get; set; }
        public virtual Semester? Semesters { get; set; }
        public virtual User? User { get; set; }
    }
}
