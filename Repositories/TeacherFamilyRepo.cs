using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
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
            return _context.TeacherFamilies
                .Where(tf => !tf.IsDeleted) // Chỉ lấy những bản ghi chưa bị xóa mềm
                .ToList();
        }

        public TeacherFamily? GetTeacherFamilyById(long id)
        {
            return _context.TeacherFamilies
                .AsNoTracking()
                .FirstOrDefault(tf => tf.Id == id && !tf.IsDeleted);
        }

        public TeacherFamily CreateTeacherFamily(TeacherFamily teacherFamily)
        {
            try
            {
                _context.TeacherFamilies.Add(teacherFamily);
                _context.SaveChanges();
                return teacherFamily;
            }
            catch (DbUpdateException ex)
            {
                // Log lỗi chi tiết để debug
                Console.WriteLine($"Lỗi DbUpdateException: {ex.InnerException?.Message}");
                throw;
            }
        }

        public TeacherFamily? UpdateTeacherFamily(TeacherFamily teacherFamily)
        {
            var existing = _context.TeacherFamilies.FirstOrDefault(tf => tf.Id == teacherFamily.Id && !tf.IsDeleted);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(teacherFamily);
            _context.SaveChanges();
            return teacherFamily;
        }

        public bool DeleteTeacherFamily(long id)
        {
            var teacherFamily = _context.TeacherFamilies.FirstOrDefault(tf => tf.Id == id && !tf.IsDeleted);
            if (teacherFamily == null) return false;

            teacherFamily.IsDeleted = true; // Xóa mềm bằng cách đặt IsDeleted = true
            _context.TeacherFamilies.Update(teacherFamily);
            return _context.SaveChanges() > 0;
        }
    }
}
