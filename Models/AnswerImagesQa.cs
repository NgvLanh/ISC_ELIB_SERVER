using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class AnswerImagesQa
    {
        public long Id { get; set; }
        public long AnswerId { get; set; }
        public string? ImageUrl { get; set; }

        public virtual AnswersQa Answer { get; set; } = null!;
    }
}
