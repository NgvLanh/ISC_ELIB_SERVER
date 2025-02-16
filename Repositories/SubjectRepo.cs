using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SubjectRepo
    {
        private readonly isc_dbContext _context;

        public SubjectRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Subject> GetAllSubject()
        {
            return _context.Subjects.ToList();
        }

        public Subject GetSubjectById(long id)
        {
            return _context.Subjects.FirstOrDefault(x => x.Id == id);
        }

        public Subject CreateSubject(Subject Subject)
        {
            _context.Subjects.Add(Subject);
            _context.SaveChanges();
            return Subject;
        }

        public Subject UpdateSubject(Subject Subject)
        {
            _context.Subjects.Update(Subject);
            _context.SaveChanges();
            return Subject;
        }

        public bool DeleteSubject(long id)
        {
            var Subject = GetSubjectById(id);
            if (Subject != null)
            {
                _context.Subjects.Remove(Subject);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
