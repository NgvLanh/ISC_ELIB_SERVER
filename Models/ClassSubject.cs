
namespace ISC_ELIB_SERVER.Models
{
    public partial class ClassSubject
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Subject? Subject { get; set; }
        public int HoursSemester1 { get; internal set; }
        public int HoursSemester2 { get; internal set; }
    }
}
