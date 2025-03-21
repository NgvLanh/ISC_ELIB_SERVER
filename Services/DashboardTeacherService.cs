using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class DashboardTeacherService : IDashboardTeacherService
    {
        private readonly DashboardTeacherRepo _dashboardTeacherRepo;

        public DashboardTeacherService(DashboardTeacherRepo dashboardTeacherRepo)
        {
            _dashboardTeacherRepo = dashboardTeacherRepo;
        }

        public DashboardOverviewResponse GetDashboardOverview(int teacherId)
        {
            return _dashboardTeacherRepo.GetDashboardOverview(teacherId);
        }

        public StudentStatisticsResponse GetStudentStatistics(int teacherId)
        {
            return _dashboardTeacherRepo.GetStudentStatistics(teacherId);
        }
    }
}
