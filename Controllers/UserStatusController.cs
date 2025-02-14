using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;


namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/user-statuses")]
    public class UserStatusController : ControllerBase
    {
        private readonly IUserStatusService _service;

        public UserStatusController(IUserStatusService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetUserStatuses([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetUserStatuses(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserStatusById(long id)
        {
            var response = _service.GetUserStatusById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateUserStatus([FromBody] UserStatusRequest userStatusRequest)
        {
            var response = _service.CreateUserStatus(userStatusRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserStatus(long id, [FromBody] UserStatus userStatus)
        {

            return Ok(ApiResponse<object>.Success("Chưa làm"));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserStatus(long id)
        {
            return Ok(ApiResponse<object>.Success("Chưa làm"));
        }

    }
}
