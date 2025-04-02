
namespace ISC_ELIB_SERVER.Models
{
    public class SubjectSubjectGroup
    {
        public int Id { get; set; }
        public int? SubjectId { get; set; }
        public int? SubjectGroupId { get; set; }

        public virtual Subject? Subject { get; set; }
        public virtual SubjectGroup? SubjectGroup { get; set; }
    }
}
