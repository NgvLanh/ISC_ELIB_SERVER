using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class AnswerImagesQa
    {
        public int Id { get; set; }
        public int? AnswerId { get; set; }
        public string? ImageUrl { get; set; }
        public bool Active { get; set; }

        public virtual AnswersQa? Answer { get; set; }
    }
}
