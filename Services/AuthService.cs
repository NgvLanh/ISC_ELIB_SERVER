using BTBackendOnline2.Models;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using DotNetEnv;

namespace ISC_ELIB_SERVER.Services
{
    public class AuthService : ILoginService, IRegisterService
    {
        private readonly isc_dbContext _context;
        private readonly TokenRequiment jwt;

        public AuthService(isc_dbContext context)
        {
            _context = context;

            jwt = new()
            {
                SecretKey = Env.GetString("JWT_SECRET_KEY"),
                Issuer = Env.GetString("JWT_ISSUER"),
                Audience = Env.GetString("JWT_AUDIENCE"),
                Subject = Env.GetString("JWT_SUBJECT")
            };
        }

        public ApiResponse<LoginResponse> AuthLogin(LoginReq request)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);

                if (user == null)
                {
                    return ApiResponse<LoginResponse>.Fail("Tên đăng nhập hoặc mật khẩu không đúng");
                }

                var token = GenerateTokens(user);

                if (string.IsNullOrEmpty(token.Item1))
                {
                    return ApiResponse<LoginResponse>.Fail("Không thể tạo AccessToken");
                }
                string roleName = user.Role?.Name ?? "Student";
                return ApiResponse<LoginResponse>.Success(new LoginResponse
                {
                    AccessToken = token.Item1,
                    RefreshToken = token.Item2,
                    User = new UserResponseLogin
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FullName = user.FullName,
                        // Avatar = user.Avatar,
                        Role = roleName.ToUpper()
                    }
                });
            }
            catch (Exception e)
            {
                return ApiResponse<LoginResponse>.Fail("Lỗi khi xác thực người dùng: " + e.Message);
            }

        }

        public (string, string) GenerateTokens(User user,
                                                            int accessExpire = 15,
                                                            int refreshExpire = 600)
        {

            var role = _context.Roles.FirstOrDefault(a => a.Id == user.RoleId);
            var roleName = role?.Name ?? "Student";

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim("Id", user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenAccess = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(accessExpire),
                signingCredentials: signIn
            );

            var tokenRefresh = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(refreshExpire),
                signingCredentials: signIn
            );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenAccess);
            string refreshToken = new JwtSecurityTokenHandler().WriteToken(tokenRefresh);

            return (accessToken, refreshToken);
        }

        public (string?, RefreshToken?) GenerateTokens(User user,
                                                            RefreshToken comparedToken,
                                                            int accessExpire = 15,
                                                            int refreshExpire = 600)
        {
            if (comparedToken == null || comparedToken.ExpireDate < DateTime.UtcNow)
            {
                return (null, null);
            }

            var role = _context.Roles.FirstOrDefault(a => a.Id == user.RoleId);

            if (role == null || string.IsNullOrEmpty(role.Name))
            {
                throw new Exception("User role is not valid");
            }

            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                throw new Exception("User is not valid");
            }

            var claims = new[] {
                 new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                 new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToString()),
                 new Claim(ClaimTypes.Name, user.Email),
                 new Claim(ClaimTypes.Role, role.Name),
                 new Claim("Id", user.Id.ToString()),
                 new Claim("Email", user.Email) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(accessExpire),
                signingCredentials: signIn
             );
            string accessTokens = new JwtSecurityTokenHandler().WriteToken(token);

            RefreshToken refreshToken = new()
            {
                Token = Guid.NewGuid().ToString(),
                ExpireDate = DateTime.UtcNow.AddMinutes(refreshExpire),
                Email = user.Email
            };
            return (accessTokens, refreshToken);
        }

        public static string ComputeSha256(string input)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + "ledang"));
            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
        public ApiResponse<RegisterResponse> Register(RegisterRequest request)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    return ApiResponse<RegisterResponse>.Fail("Email đã tồn tại.");
                }

                var role = _context.Roles.FirstOrDefault(r => r.Id == request.RoleId);
                if (role == null)
                {
                    return ApiResponse<RegisterResponse>.Fail("Vai trò không hợp lệ.");
                }

                string hashedPassword = ComputeSha256(request.Password);

                var newUser = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Password = hashedPassword,
                    RoleId = request.RoleId
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                var response = new RegisterResponse
                {
                    Id = newUser.Id,
                    FullName = newUser.FullName,
                    Email = newUser.Email,
                    RoleId = newUser.RoleId ?? 3,
                    CreatedAt = DateTime.UtcNow
                };


                return ApiResponse<RegisterResponse>.Success(response);
            }
            catch (Exception e)
            {
                return ApiResponse<RegisterResponse>.Fail("Lỗi khi đăng ký người dùng: " + e.Message);
            }
        }

    }
}
