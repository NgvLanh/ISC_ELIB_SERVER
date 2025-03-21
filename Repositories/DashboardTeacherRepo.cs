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
                TotalCourses = _context.TeachingAssignments.Count(c => c.UserId == teacherId && c.Active),
                TotalOnlineClasses = _context.Sessions.Count(s => s.TeachingAssignment.UserId == teacherId && s.Active),
                TotalPendingTests = _context.TestsSubmissions.Count(t => t.User.Id == teacherId && !t.Graded.GetValueOrDefault()),
                TotalQA = _context.QuestionQas.Count(q => q.UserId == teacherId)
            };
        }

        public StudentStatisticsResponse GetStudentStatistics(int teacherId)
        {
            return new StudentStatisticsResponse
            {
                TotalClasses = _context.TeachingAssignments.Count(c => c.UserId == teacherId && c.Active),
                ExcellentStudents = _context.StudentScores.Count(t => t.User.Id == teacherId && t.Score >= 9),
                GoodStudents = _context.StudentScores.Count(t => t.User.Id == teacherId && t.Score >= 7 && t.Score < 9),
                AverageStudents = _context.StudentScores.Count(t => t.User.Id == teacherId && t.Score >= 5 && t.Score < 7),
                WeakStudents = _context.StudentScores.Count(t => t.User.Id == teacherId && t.Score < 5)
            };
        }
    }
}
