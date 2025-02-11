using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestFile
    {
        public long Id { get; set; }
        public long TestId { get; set; }
        public string? FileUrl { get; set; }

        public virtual Test Test { get; set; } = null!;
    }
}
