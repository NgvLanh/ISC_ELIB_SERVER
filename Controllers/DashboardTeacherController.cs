using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Controllers
{
    [Authorize] // Yêu cầu đăng nhập
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardTeacherController : ControllerBase
    {
        private readonly IDashboardTeacherService _dashboardTeacherService;
        private readonly IUserService _userService;

        public DashboardTeacherController(IDashboardTeacherService dashboardTeacherService, IUserService userService)
        {
            _dashboardTeacherService = dashboardTeacherService;
            _userService = userService;
        }

        // API lấy tổng quan dashboard theo user đăng nhập
        [HttpGet("overview")]
        public IActionResult GetDashboardOverview()
        {
            var userIdResult = GetUserId();
            if (userIdResult is UnauthorizedResult)
                return Unauthorized();

            var userId = (userIdResult as OkObjectResult)?.Value as int? ?? 0;
            var response = _dashboardTeacherService.GetDashboardOverview(userId);
            return Ok(response);
        }

        // API lấy thống kê học sinh theo user đăng nhập
        [HttpGet("student-statistics")]
        public IActionResult GetStudentStatistics()
        {
            var userIdResult = GetUserId();
            if (userIdResult is UnauthorizedResult)
                return Unauthorized();

            var userId = (userIdResult as OkObjectResult)?.Value as int? ?? 0;
            var response = _dashboardTeacherService.GetStudentStatistics(userId);
            return Ok(response);
        }

        // Lấy userId từ token JWT
        private IActionResult GetUserId()
        {
            var userId = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Fail("Không tìm thấy ID trong token"));
            }

            var user = _userService.GetUserById(int.Parse(userId));

            if (user == null)
            {
                return BadRequest(ApiResponse<string>.Fail("User not found"));
            }

            return Ok(int.Parse(userId));
        }
    }
}

