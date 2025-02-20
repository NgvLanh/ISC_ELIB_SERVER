using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class QuestionQa
    {
        public QuestionQa()
        {
            AnswersQas = new HashSet<AnswersQa>();
            QuestionImagesQas = new HashSet<QuestionImagesQa>();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? SubjectId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Active { get; set; }

        public virtual Subject? Subject { get; set; }
        public virtual ICollection<AnswersQa> AnswersQas { get; set; }
        public virtual ICollection<QuestionImagesQa> QuestionImagesQas { get; set; }
    }
}
