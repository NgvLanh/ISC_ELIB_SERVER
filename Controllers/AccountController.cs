using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AccountController : ControllerBase
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public AccountController(IPasswordResetService passwordResetService, IEmailService emailService, IUserService userService)
    {
        _passwordResetService = passwordResetService;
        _emailService = emailService;
        _userService = userService;
    }

    [HttpPost("request-password-reset")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest request)
    {
        var user = await _passwordResetService.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("Email không đúng");
        }

        var otp = new Random().Next(100000, 999999).ToString();
        await _passwordResetService.SaveOtpAsync(user.Id, otp);

        await _emailService.SendEmailAsync(user.Email, "Mã OTP", otp);

        return Ok("OTP đã được gửi đến email của bạn");
    }

    [HttpPost("verify-otp")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyOtp([FromBody] PasswordResetOtpVerificationRequest request)
    {
        var user = await _passwordResetService.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("Email không đúng");
        }

        var isValidOtp = await _passwordResetService.VerifyOtpAsync(user.Id, request.Otp);
        if (!isValidOtp)
        {
            return BadRequest("OTP không hợp lệ");
        }

        // Tạo mật khẩu tạm thời
        var temporaryPassword = GenerateRandomPassword();

        // Cập nhật mật khẩu tạm thời trong cơ sở dữ liệu
        await _userService.UpdateUserPassword(user.Id, temporaryPassword);

        // Gửi mật khẩu tạm thời qua email
        await _emailService.SendEmailAsync(user.Email, "Mật khẩu tạm thời của bạn", temporaryPassword);

        return Ok("Mật khẩu tạm thời đã được gửi đến email của bạn");
    }

    private string GenerateRandomPassword()
    {
        const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder sb = new StringBuilder();
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (sb.Length < 8)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                sb.Append(validChars[(int)(num % (uint)validChars.Length)]);
            }
        }

        return sb.ToString();
    }
}
