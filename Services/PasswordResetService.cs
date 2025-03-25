using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class PasswordResetService : IPasswordResetService
{
    private readonly isc_dbContext _context;

    public PasswordResetService(isc_dbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task SaveOtpAsync(int userId, string otp)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.SetOtp(otp);
            user.SetOtpExpiration(DateTime.UtcNow.AddMinutes(10)); // OTP có hiệu lực trong 10 phút
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> VerifyOtpAsync(int userId, string otp)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            return user.GetOtp() == otp && user.GetOtpExpiration() > DateTime.UtcNow;
        }
        return false;
    }
}
