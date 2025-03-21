using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class DashboardTeacherRepo
    {
        private readonly isc_dbContext _context;

        public DashboardTeacherRepo(isc_dbContext context)
        {
            _context = context;
        }

        public DashboardOverviewResponse GetDashboardOverview(int teacherId)
        {
            return new DashboardOverviewResponse
            {
                // Tổng số khóa học mà giáo viên đang dạy
                TotalCourses = _context.TeachingAssignments
                    .Where(c => c.UserId == teacherId && c.Active)
                    .Select(c => c.SubjectId) // Đếm theo môn học (khóa học)
                    .Distinct()
                    .Count(),

                // Tổng số lớp học online mà giáo viên tạo
                TotalOnlineClasses = _context.Sessions
                    .Where(s => s.TeachingAssignment.UserId == teacherId && s.Active)
                    .Count(),

                // Tổng số bài kiểm tra chưa chấm
                TotalPendingTests = _context.TestsSubmissions
                    .Where(t => t.Test.UserId == teacherId && !t.Graded.GetValueOrDefault() && t.Active.GetValueOrDefault())
                    .Count(),

                // Tổng số câu hỏi trong mục hỏi đáp mà giáo viên tham gia
                TotalQA = _context.QuestionQas
                    .Where(q => q.UserId == teacherId && q.Active)
                    .Count()
            };
        }

        public StudentStatisticsResponse GetStudentStatistics(int teacherId)
        {
            // Lấy danh sách các lớp giáo viên đang dạy
            var classIds = _context.TeachingAssignments
                .Where(c => c.UserId == teacherId && c.Active)
                .Select(c => c.ClassId)
                .Distinct()
                .ToList();

            // Lấy danh sách học sinh trong các lớp đó
            var studentScores = _context.StudentScores
                .Where(s => classIds.Contains(s.User.ClassId) && s.Active);

            return new StudentStatisticsResponse
            {
                TotalClasses = classIds.Count,
                ExcellentStudents = studentScores.Count(s => s.Score >= 9),
                GoodStudents = studentScores.Count(s => s.Score >= 7 && s.Score < 9),
                AverageStudents = studentScores.Count(s => s.Score >= 5 && s.Score < 7),
                WeakStudents = studentScores.Count(s => s.Score < 5)
            };
        }
    }
}
