using BTBackendOnline2.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class LoginRes
    {
        public string? AccessToken { get; set; }
        public RefreshToken? RefreshTokens { get; set; }
    }
}
