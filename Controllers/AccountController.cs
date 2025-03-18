using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IPasswordResetService _passwordResetService;
    private readonly IEmailService _emailService;

    public AccountController(IPasswordResetService passwordResetService, IEmailService emailService)
    {
        _passwordResetService = passwordResetService;
        _emailService = emailService;
    }

    [HttpPost("request-password-reset")]
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

        await _emailService.SendEmailAsync(user.Email, "Mật khẩu của bạn", user.Password);

        return Ok("Mật khẩu đã được gửi đến email của bạn");
    }
}
