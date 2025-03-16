using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _service;
        private readonly IMapper _mapper;


        public AuthController(ILoginService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginReq request)
        {
            var response = _service.AuthLogin(request);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPost("GetUserIdByAccessToken")]
        public IActionResult GetUserIdByAccessToken([FromBody] AccessTokenReq request)
        {
            var accessTokenRes = _mapper.Map<AccessTokenReq>(request);

            if (string.IsNullOrEmpty(accessTokenRes.AccessToken))
            {
                return BadRequest(new { message = "AccessToken is required." });
            }

            try
            {
                var payload = ExtractAccessTokenJWT.DecodeJWT(accessTokenRes);

                if (payload != null && payload.ContainsKey("Id"))
                {
                    return Ok(new { UserId = payload["Id"] });
                }
                else
                {
                    return BadRequest(new { message = "UserId not found in token." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid token.", error = ex.Message });
            }
        }
    }
}
