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

        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Active { get; set; }

        public virtual QuestionQa? Question { get; set; }
        public virtual ICollection<AnswerImagesQa> AnswerImagesQas { get; set; }
    }
}
