using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class QuestionImagesQa
    {
        public long Id { get; set; }
        public long QuestionId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual QuestionQa Question { get; set; } = null!;
    }
}
