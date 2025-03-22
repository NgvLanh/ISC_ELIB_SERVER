using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _service;
        private readonly IUserService _userService;

        public AuthController(ILoginService service, IMapper mapper, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginReq request)
        {
            var response = _service.AuthLogin(request);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [Authorize]
        [HttpGet("verify-token")]
        public IActionResult VerifyToken()
        {
            var userId = User.FindFirst("Id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Fail("Không tìm thấy ID trong token"));
            }

            var user = _userService.GetUserById(int.Parse(userId));
            
            if(user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }
    }
}
