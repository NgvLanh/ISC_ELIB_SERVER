using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ILoginService
    {
        ApiResponse<LoginRes> AuthLogin(LoginReq request);
    }
}
