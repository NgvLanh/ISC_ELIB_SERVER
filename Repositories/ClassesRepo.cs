using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IClassesRepo
    {
        IQueryable<Class> GetClass();
        Class GetClassById(int id);
        Class CreateClass(Class classes);
        Class? UpdateClass(Class classes);
        bool DeleteClass(int id);

    }

    public class ClassRepo : IClassesRepo
    {
        private readonly isc_dbContext _context;

        public ClassRepo(isc_dbContext context)
        {
            _context = context;
        }

        public IQueryable<Class> GetClass()
        {
            return _context.Classes
                .AsNoTracking()
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType);
        }




        public Class? GetClassById(int id)
        {
            return _context.Classes
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType)
                .FirstOrDefault(c => c.Id == id);
        }






        public Class CreateClass(Class newClass)
        {
            _context.Classes.Add(newClass);
            _context.SaveChanges();
            return _context.Classes
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType)
                .FirstOrDefault(c => c.Id == newClass.Id);
        }

        public Class? UpdateClass(Class updatedClass)
        {
            var existingClass = _context.Classes
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType)
                .FirstOrDefault(c => c.Id == updatedClass.Id);

            if (existingClass == null)
            {
                return null;
            }

            existingClass.Name = updatedClass.Name;
            existingClass.Description = updatedClass.Description;
            existingClass.StudentQuantity = updatedClass.StudentQuantity;
            existingClass.SubjectQuantity = updatedClass.SubjectQuantity;
            existingClass.GradeLevelId = updatedClass.GradeLevelId;
            existingClass.AcademicYearId = updatedClass.AcademicYearId;

            _context.SaveChanges();

            return _context.Classes
                .Include(c => c.GradeLevel)
                .Include(c => c.AcademicYear)
                .Include(c => c.User)
                .Include(c => c.ClassType)
                .FirstOrDefault(c => c.Id == updatedClass.Id);
        }


        public bool DeleteClass(int id)
        {
            var existingClass = _context.Classes.Find(id);

            if (existingClass == null)
            {
                return false;
            }

            _context.Classes.Update(existingClass);
            return _context.SaveChanges() > 0;
        }
    }
}
