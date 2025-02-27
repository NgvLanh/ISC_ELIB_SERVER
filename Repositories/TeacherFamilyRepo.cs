using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TeacherFamilyRepo
    {
        private readonly isc_dbContext _context;
        public TeacherFamilyRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<TeacherFamily> GetTeacherFamilies()
        {
            return _context.TeacherFamilies.Where(tf => !tf.IsDeleted).ToList();
        }

        public TeacherFamily? GetTeacherFamilyById(long id)
        {
            return _context.TeacherFamilies.FirstOrDefault(tf => tf.Id == id && !tf.IsDeleted);
        }

        public TeacherFamily CreateTeacherFamily(TeacherFamily teacherFamily)
        {
            _context.TeacherFamilies.Add(teacherFamily);
            _context.SaveChanges();
            return teacherFamily;
        }

        public TeacherFamily? UpdateTeacherFamily(TeacherFamily teacherFamily)
        {
            var existing = GetTeacherFamilyById(teacherFamily.Id);
            if (existing == null) return null;

            _context.TeacherFamilies.Update(teacherFamily);
            _context.SaveChanges();
            return teacherFamily;
        }

        public bool DeleteTeacherFamily(long id)
        {
            var teacherFamily = GetTeacherFamilyById(id);
            if (teacherFamily == null) return false;

            teacherFamily.IsDeleted = true;
            _context.TeacherFamilies.Update(teacherFamily);
            return _context.SaveChanges() > 0;
        }
    }
}
