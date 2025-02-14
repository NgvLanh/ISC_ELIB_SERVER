using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TeacherInfoRepo
    {
        private readonly isc_elibContext _context;

        // Constructor nhận context
        public TeacherInfoRepo(isc_elibContext context)
        {
            _context = context;
        }

        // Create: Thêm mới TeacherInfo vào database
        public void AddTeacherInfo(TeacherInfo teacherInfo)
        {
            _context.TeacherInfos.Add(teacherInfo);
            _context.SaveChanges();
        }

        // Read: Lấy tất cả TeacherInfos từ database
        public List<TeacherInfo> GetAllTeacherInfo()
        {
            return _context.TeacherInfos.ToList();
        }

        // Read: Lấy TeacherInfo theo Id
        public TeacherInfo GetTeacherInfoById(long id)
        {
            return _context.TeacherInfos.FirstOrDefault(t => t.Id == id);
        }

        // Update: Cập nhật thông tin TeacherInfo
        public void UpdateTeacherInfo(TeacherInfo teacherInfo)
        {
            _context.TeacherInfos.Update(teacherInfo);
            _context.SaveChanges();
        }

        // Delete: Xóa TeacherInfo theo Id
        public void DeleteTeacherInfo(long id)
        {
            var teacherInfo = _context.TeacherInfos.FirstOrDefault(t => t.Id == id);
            if (teacherInfo != null)
            {
                _context.TeacherInfos.Remove(teacherInfo);
                _context.SaveChanges();
            }
        }
    }
}
