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

        public AuthController(ILoginService service, IMapper mapper)
        {
            _service = service;
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

            return Ok(ApiResponse<string>.Success($"User ID: {userId}"));
        }
    }
}
