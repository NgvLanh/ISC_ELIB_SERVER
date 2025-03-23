using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IClassesRepo
    {
        ICollection<Class> GetClass();
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

        public ICollection<Class> GetClass()
        {
            return _context.Classes.ToList();
        }

        public Class GetClassById(int id)
        {
            return _context.Classes.FirstOrDefault(c => c.Id == id);
        }

        public Class CreateClass(Class newClass)
        {
            _context.Classes.Add(newClass);
            _context.SaveChanges();
            return newClass;
        }

        public Class? UpdateClass(Class updatedClass)
        {
            var existingClass = _context.Classes.Find(updatedClass.Id);

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
            return existingClass;
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
