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
        public StudentInfo GetStudentInfoById(int userId)
        {
            return _context.StudentInfos.
                Include(s => s.User)  // Load User liên kết với StudentInfo
                    .FirstOrDefault(s => s.UserId == userId);
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
                studentInfo.Active = false;
                _context.SaveChanges();
            }
        }

        // Lọc theo thông tin bảng UserId
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

        // Lấy danh sách học viên theo lớp với thông tin đầy đủ
        public List<StudentInfo> GetStudentsByClass(int classId)
        {
            return _context.StudentInfos
                .Include(s => s.User)
                    .ThenInclude(u => u.AcademicYear) // Load AcademicYear từ User
                .Include(s => s.User)
                    .ThenInclude(u => u.UserStatus) // Load UserStatus từ User
                .Where(s => s.User != null && s.User.ClassId == classId)
                .ToList();
        }

        // Lấy danh sách học viên theo user figma
        public List<StudentInfo> GetAllStudents()
        {
            var students = _context.StudentInfos
                .Include(s => s.User)
                    .ThenInclude(u => u.Class)
                .Include(s => s.User)
                    .ThenInclude(u => u.UserStatus)
                .Include(s => s.User)
                    .ThenInclude(u => u.AcademicYear)
                        .ThenInclude(a => a.Semesters)
                .AsNoTracking()
                .ToList()
                .GroupBy(s => s.UserId)
                .Select(g => g.First()) // loại bỏ trùng
                .ToList();

            return students;
        }

    }
}