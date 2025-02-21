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
    }
}