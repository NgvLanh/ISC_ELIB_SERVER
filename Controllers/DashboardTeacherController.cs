using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/dashboard-teacher")]
    [ApiController]
    [Authorize]
    public class DashboardTeacherController : ControllerBase
    {
        private readonly IDashboardTeacherService _dashboardTeacherService;

        public DashboardTeacherController(IDashboardTeacherService dashboardTeacherService)
        {
            _dashboardTeacherService = dashboardTeacherService;
        }

        [HttpGet("overview")]
        public ActionResult<DashboardOverviewResponse> GetDashboardOverview()
        {
            int? teacherId = GetUserIdFromToken();
            if (teacherId == null) return Unauthorized();

            var result = _dashboardTeacherService.GetDashboardOverview(teacherId.Value);
            return Ok(result);
        }

        [HttpGet("student-statistics")]
        public ActionResult<StudentStatisticsResponse> GetStudentStatistics()
        {
            int? teacherId = GetUserIdFromToken();
            if (teacherId == null) return Unauthorized();

            var result = _dashboardTeacherService.GetStudentStatistics(teacherId.Value);
            return Ok(result);
        }

        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null) return null;

            return int.TryParse(userIdClaim.Value, out int userId) ? userId : null;
        }
    }
}
