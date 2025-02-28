using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class GradeLevelRepo
    {
        private readonly isc_dbContext _context;
        public GradeLevelRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<GradeLevel> GetGradeLevels()
        {
            return _context.GradeLevels
                .Where(a => a.Active)
                .ToList();
        }

        public GradeLevel GetGradeLevelById(long id)
        {
            return _context.GradeLevels.Where(a => a.Active).FirstOrDefault(s => s.Id == id);
        }

        public GradeLevel CreateGradeLevel(GradeLevel GradeLevel)
        {
            _context.GradeLevels.Add(GradeLevel);
            _context.SaveChanges();
            return GradeLevel;
        }

        public GradeLevel UpdateGradeLevel(GradeLevel GradeLevel)
        {
            _context.GradeLevels.Update(GradeLevel);
            _context.SaveChanges();
            return GradeLevel;
        }

        public bool DeleteGradeLevel(long id)
        {
            var GradeLevel = GetGradeLevelById(id);
            if (GradeLevel != null)
            {
                GradeLevel.Active = false;
                _context.GradeLevels.Update(GradeLevel);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}