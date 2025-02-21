using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamGraderRepo
    {
        private readonly isc_dbContext _context;

        public ExamGraderRepo(isc_dbContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ ExamGraders
        public ICollection<ExamGrader> GetExamGraders()
        {
            return _context.ExamGraders.ToList();
        }

        // Lấy ExamGrader theo Id
        public ExamGrader GetExamGraderById(int id)
        {
            return _context.ExamGraders.FirstOrDefault(eg => eg.Id == id);
        }

        // Lấy danh sách ExamGraders theo ExamId (có thể mở rộng thêm các điều kiện khác nếu cần)
        public ICollection<ExamGrader> GetExamGradersByExamId(int examId)
        {
            return _context.ExamGraders.Where(eg => eg.ExamId == examId).ToList();
        }

        // Thêm mới ExamGrader
        public ExamGrader CreateExamGrader(ExamGrader examGrader)
        {
            _context.ExamGraders.Add(examGrader);
            _context.SaveChanges();
            return examGrader;
        }

        // Cập nhật ExamGrader
        public ExamGrader UpdateExamGrader(ExamGrader examGrader)
        {
            _context.ExamGraders.Update(examGrader);
            _context.SaveChanges();
            return examGrader;
        }

        // Xoá mềm: thay đổi Active và lưu thay đổi
        public bool DeleteExamGrader(int id)
        {
            var examGrader = GetExamGraderById(id);
            if (examGrader != null)
            {
                examGrader.Active = !examGrader.Active;
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        // Phương thức hỗ trợ tách entity khỏi context nếu cần
        public void Detach<T>(T entity) where T : class
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }
    }
}
