using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IDashboardTeacherService
    {
        DashboardOverviewResponse GetDashboardOverview(int userId);
        StudentStatisticsResponse GetStudentStatistics(int userId);
    }
}
