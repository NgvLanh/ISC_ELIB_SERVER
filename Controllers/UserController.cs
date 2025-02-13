using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult<ApiResponse<ICollection<UserResponse>>> GetUsers(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "",
            [FromQuery] string sortColumn = "id",
            [FromQuery] string sortOrder = "asc")
        {
            return Ok(_userService.GetUsers(page, pageSize, search, sortColumn, sortOrder));
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<UserResponse>> GetUserById(long id)
        {
            return Ok(_userService.GetUserById(id));
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<ApiResponse<UserResponse>> CreateUser([FromBody] UserRequest userRequest)
        {
            if (userRequest == null)
                return BadRequest("Invalid user data.");

            // Convert DateTime fields to Unspecified kind before saving
            userRequest.Dob = DateTimeUtils.ConvertToUnspecified(userRequest.Dob);
            userRequest.EnrollmentDate = DateTimeUtils.ConvertToUnspecified(userRequest.EnrollmentDate);

            return Ok(_userService.CreateUser(userRequest));
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<UserResponse>> UpdateUser(long id, [FromBody] UserRequest userRequest)
        {
            if (userRequest == null || id != userRequest.Id)
                return BadRequest("User ID mismatch.");

            // Convert DateTime fields to Unspecified kind before updating
            userRequest.Dob = DateTimeUtils.ConvertToUnspecified(userRequest.Dob);
            userRequest.EnrollmentDate = DateTimeUtils.ConvertToUnspecified(userRequest.EnrollmentDate);

            return Ok(_userService.UpdateUser(userRequest));
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<User>> DeleteUser(long id)
        {
            return Ok(_userService.DeleteUser(id));
        }
    }
}
