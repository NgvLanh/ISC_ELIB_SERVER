using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
namespace ISC_ELIB_SERVER.Models
{
    [Table("question_qa")]
    public partial class QuestionQa
    {
       
        public QuestionQa()
        {
            AnswersQas = new HashSet<AnswersQa>();
            QuestionImagesQas = new HashSet<QuestionImagesQa>();
        }
        
        public long Id { get; set; }
        public long UserId { get; set; }
        public long SubjectId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Subject Subject { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<AnswersQa> AnswersQas { get; set; }
        public virtual ICollection<QuestionImagesQa> QuestionImagesQas { get; set; }
    }
}
