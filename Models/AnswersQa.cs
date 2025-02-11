using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class AnswersQa
    {
        public AnswersQa()
        {
            AnswerImagesQas = new HashSet<AnswerImagesQa>();
        }

        public long Id { get; set; }
        public long QuestionId { get; set; }
        public long UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual QuestionQa Question { get; set; } = null!;
        public virtual ICollection<AnswerImagesQa> AnswerImagesQas { get; set; }
    }
}
