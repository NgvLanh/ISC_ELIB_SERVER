using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AcademicYearRepo
    {
        private readonly isc_dbContext _context;
        public AcademicYearRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<AcademicYear> GetAcademicYears()
        {
            return _context.AcademicYears
                .Where(a => a.Active)
                .ToList();
        }

        public AcademicYear GetAcademicYearById(long id)
        {
            return _context.AcademicYears.Where(a => a.Active).FirstOrDefault(s => s.Id == id);
        }

        public AcademicYear CreateAcademicYear(AcademicYear academicYear)
        {
            _context.AcademicYears.Add(academicYear);
            _context.SaveChanges();
            return academicYear;
        }

        public AcademicYear UpdateAcademicYear(AcademicYear academicYear)
        {
            _context.AcademicYears.Update(academicYear);
            _context.SaveChanges();
            return academicYear;
        }

        public bool DeleteAcademicYear(long id)
        {
            var academicYear = GetAcademicYearById(id);
            if (academicYear != null)
            {
                academicYear.Active = false;
                _context.AcademicYears.Update(academicYear);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}