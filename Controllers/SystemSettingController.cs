using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/system-settings")]
    public class SystemSettingController : ControllerBase
    {
        private readonly ISystemSettingsService _service;

        public SystemSettingController(ISystemSettingsService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetSystemSettings([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = _service.GetSystemSettings(page, pageSize);
            return StatusCode(response.Code, response);
        }

        [HttpGet("user")]
        [Authorize]
        public IActionResult GetSystemSettingByUserId()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = _service.GetSystemSettingByUserId(userId);
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateOrUpdateSystemSetting([FromBody] SystemSettingRequest request)
        {
            if (request == null)
            {
                return BadRequest(ApiResponse<object>.BadRequest("Dữ liệu không hợp lệ"));
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = _service.CreateOrUpdateSystemSetting(request, userId);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteSystemSetting(int id)
        {
            var response = _service.DeleteSystemSetting(id);
            return StatusCode(response.Code, response);
        }
    }
}
