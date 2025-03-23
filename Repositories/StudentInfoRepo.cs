using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class StudentInfoRepo
    {
        private readonly isc_dbContext _context;

        // Constructor nhận context
        public StudentInfoRepo(isc_dbContext context)
        {
            _context = context;
        }

        // Create: Thêm mới StudentInfo vào database
        public void AddStudentInfo(StudentInfo studentInfo)
        {
            _context.StudentInfos.Add(studentInfo);
            _context.SaveChanges();
        }

        // Read: Lấy tất cả StudentInfos từ database
        public List<StudentInfo> GetAllStudentInfo()
        {
            return _context.StudentInfos.ToList();
        }

        // Read: Lấy StudentInfo theo Id
        public StudentInfo GetStudentInfoById(int id)
        {
            return _context.StudentInfos.FirstOrDefault(s => s.Id == id);
        }

        // Update: Cập nhật thông tin StudentInfo
        public void UpdateStudentInfo(StudentInfo studentInfo)
        {
            _context.StudentInfos.Update(studentInfo);
            _context.SaveChanges();
        }

        // Delete: Xóa StudentInfo theo Id
        public void DeleteStudentInfo(int id)
        {
            var studentInfo = _context.StudentInfos.FirstOrDefault(s => s.Id == id);
            if (studentInfo != null)
            {
                _context.StudentInfos.Remove(studentInfo);
                _context.SaveChanges();
            }
        }

        // Lọc theo ClassId
        public List<StudentInfo> GetStudentInfoByClassId(int classId)
        {
            return _context.StudentInfos
                .Include(si => si.User)  // Đảm bảo lấy dữ liệu User
                .Where(si => si.User != null && si.User.ClassId == classId)
                .ToList();
        }

        // Lọc theo UserId
        public List<StudentInfo> GetStudentInfosByUserId(int userId)
        {
            return _context.StudentInfos
                .Include(s => s.User)  // Load User liên kết với StudentInfo
                    .ThenInclude(u => u.Class)  // Load Class từ User
                .Include(s => s.User)
                    .ThenInclude(u => u.UserStatus)  // Load UserStatus từ User
                .Where(s => s.UserId == userId)
                .ToList();
        }
    }
}