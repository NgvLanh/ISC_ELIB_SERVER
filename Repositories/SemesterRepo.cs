using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SemesterRepo
    {
        private readonly isc_dbContext _context;
        public SemesterRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Semester> GetSemesters()
        {
            return _context.Semesters
                .Where(a => a.Active)
                .ToList();
        }

        public Semester GetSemesterById(long id)
        {
            return _context.Semesters.Where(a => a.Active).FirstOrDefault(s => s.Id == id);
        }

        public Semester CreateSemester(Semester Semester)
        {
            _context.Semesters.Add(Semester);
            _context.SaveChanges();
            return Semester;
        }

        public Semester UpdateSemester(Semester Semester)
        {
            _context.Semesters.Update(Semester);
            _context.SaveChanges();
            return Semester;
        }

        public bool DeleteSemester(long id)
        {
            var Semester = GetSemesterById(id);
            if (Semester != null)
            {
                Semester.Active = false;
                _context.Semesters.Update(Semester);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}