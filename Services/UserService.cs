using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ISC_ELIB_SERVER.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepo _userRepo;
        private readonly RoleRepo _roleRepo;
        private readonly AcademicYearRepo _academicYearRepo;
        private readonly UserStatusRepo _userStatusRepo;
        private readonly ClassRepo _classRepo;
        private readonly IMapper _mapper;
        private readonly GhnService _ghnService;

        public UserService(UserRepo userRepo, RoleRepo roleRepo, AcademicYearRepo academicYearRepo,
            UserStatusRepo userStatusRepo, ClassRepo classRepo, IMapper mapper, GhnService ghnService)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _academicYearRepo = academicYearRepo;
            _userStatusRepo = userStatusRepo;
            _classRepo = classRepo;
            _mapper = mapper;
            _ghnService = ghnService;
        }

        public async Task<ApiResponse<ICollection<UserResponse>>> GetUsers(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _userRepo.GetUsers().AsQueryable();

            // Tìm kiếm người dùng theo tên, email, hoặc code
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.FullName.ToLower().Contains(search.ToLower()) ||
                                          u.Email.ToLower().Contains(search.ToLower()) ||
                                          u.Code.ToLower().Contains(search.ToLower()));
            }

            // Sắp xếp theo các cột được chỉ định
            query = sortColumn switch
            {
                "FullName" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(u => u.FullName) : query.OrderBy(u => u.FullName),
                "Email" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id),
                _ => query.OrderBy(u => u.Id)
            };

            var result = query.ToList();
            var responses = new List<UserResponse>();

            // Lấy địa chỉ đầy đủ từ GHN API cho từng user
            foreach (var user in result)
            {
                var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(user.ProvinceCode ?? 0, user.DistrictCode ?? 0, user.WardCode?.ToString() ?? "");
                var response = _mapper.Map<UserResponse>(user);
                response.ProvinceName = provinceName;
                response.DistrictName = districtName;
                response.WardName = wardName;
                // RoleName bằng cách lấy từ RoleRepo
                var role = _roleRepo.GetRoleById(user.RoleId ?? 0);
                response.RoleName = role?.Name;
                responses.Add(response);
            }

            return responses.Any() ? ApiResponse<ICollection<UserResponse>>
                .Success(responses, page, pageSize, _userRepo.GetUsers().Count)
                : ApiResponse<ICollection<UserResponse>>.NotFound("Không có dữ liệu");
        }

        public async Task<ApiResponse<UserResponse>> GetUserById(int id)
        {
            var user = _userRepo.GetUserById(id);
            if (user == null) return ApiResponse<UserResponse>.NotFound($"Không tìm thấy người dùng với ID #{id}");

            var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(user.ProvinceCode ?? 0, user.DistrictCode ?? 0, user.WardCode?.ToString() ?? "");
            var response = _mapper.Map<UserResponse>(user);
            response.ProvinceName = provinceName;
            response.DistrictName = districtName;
            response.WardName = wardName;
            // RoleName bằng cách lấy từ RoleRepo
            var role = _roleRepo.GetRoleById(user.RoleId ?? 0);
            response.RoleName = role?.Name;
            return ApiResponse<UserResponse>.Success(response);
        }

        public async Task<ApiResponse<UserResponse>> GetUserByCode(string code)
        {
            var user = _userRepo.GetUsers().FirstOrDefault(u => u.Code?.ToLower() == code.ToLower());
            if (user == null) return ApiResponse<UserResponse>.NotFound($"Không tìm thấy người dùng với mã {code}");

            var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(user.ProvinceCode ?? 0, user.DistrictCode ?? 0, user.WardCode?.ToString() ?? "");
            var response = _mapper.Map<UserResponse>(user);
            response.ProvinceName = provinceName;
            response.DistrictName = districtName;
            response.WardName = wardName;
            // RoleName bằng cách lấy từ RoleRepo
            var role = _roleRepo.GetRoleById(user.RoleId ?? 0);
            response.RoleName = role?.Name;
            return ApiResponse<UserResponse>.Success(response);
        }

        public ApiResponse<int> GetQuantityUserByRoleId(int roleId)
        {

            try
            {
                if (_roleRepo.GetRoleById(roleId) == null)
                {
                    return ApiResponse<int>.BadRequest("RoleId không hợp lệ");
                }

                return ApiResponse<int>.Success(_userRepo.GetQuantityUserByRoleId(roleId));
            }
            catch
            {
                return ApiResponse<int>.Fail("Lỗi không thể lấy số lượng users");
            }

        }

        public ApiResponse<UserResponse> CreateUser(UserRequest userRequest)
        {
            // Kiểm tra RoleId có tồn tại không
            if (_roleRepo.GetRoleById(userRequest.RoleId) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("RoleId không hợp lệ");
            }

            // Kiểm tra AcademicYearId có tồn tại không (nếu có nhập)
            if (userRequest.AcademicYearId.HasValue &&
                _academicYearRepo.GetAcademicYearById(userRequest.AcademicYearId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("AcademicYearId không hợp lệ");
            }

            // Kiểm tra UserStatusId có tồn tại không (nếu có nhập)
            if (userRequest.UserStatusId.HasValue &&
                _userStatusRepo.GetUserStatusById(userRequest.UserStatusId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("UserStatusId không hợp lệ");
            }

            // Kiểm tra ClassId có tồn tại không (nếu có nhập)
            if (userRequest.ClassId.HasValue &&
                _classRepo.GetClassById(userRequest.ClassId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("ClassId không hợp lệ");
            }

            // Kiểm tra mã người dùng đã tồn tại chưa
            if (_userRepo.GetUserByCode(userRequest.Code) != null)
            {
                return ApiResponse<UserResponse>.BadRequest("Mã người dùng đã tồn tại");
            }

            // Nếu tất cả đều hợp lệ, tạo user
            var newUser = new User
            {
                Code = userRequest.Code,
                Password = ComputeSha256("a"),
                FullName = userRequest.FullName,
                Dob = userRequest.Dob,
                Gender = userRequest.Gender,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                PlaceBirth = userRequest.PlaceBirth,
                Nation = userRequest.Nation,
                Religion = userRequest.Religion,
                EnrollmentDate = userRequest.EnrollmentDate,
                RoleId = userRequest.RoleId,
                AcademicYearId = userRequest.AcademicYearId,
                UserStatusId = userRequest.UserStatusId,
                ClassId = userRequest.ClassId,
                EntryType = userRequest.EntryType,
                AddressFull = userRequest.AddressFull,
                ProvinceCode = userRequest.ProvinceCode,
                DistrictCode = userRequest.DistrictCode,
                WardCode = userRequest.WardCode,
                Street = userRequest.Street,
                Active = userRequest.Active,
                AvatarUrl = userRequest.AvatarUrl  
            };

            try
            {
                var createdUser = _userRepo.CreateUser(newUser);
                return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(createdUser));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.BadRequest(ex.Message);
            }
        }

        public ApiResponse<UserResponse> UpdateUser(int id, UserRequest userRequest)
        {
            var user = _userRepo.GetUserById(id);
            if (user == null)
            {
                return ApiResponse<UserResponse>.NotFound("Không tìm thấy người dùng để cập nhật");
            }

            // Kiểm tra RoleId có tồn tại không
            if (_roleRepo.GetRoleById(userRequest.RoleId) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("RoleId không hợp lệ");
            }

            // Kiểm tra AcademicYearId có tồn tại không (nếu có nhập)
            if (userRequest.AcademicYearId.HasValue &&
                _academicYearRepo.GetAcademicYearById(userRequest.AcademicYearId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("AcademicYearId không hợp lệ");
            }

            // Kiểm tra UserStatusId có tồn tại không (nếu có nhập)
            if (userRequest.UserStatusId.HasValue &&
                _userStatusRepo.GetUserStatusById(userRequest.UserStatusId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("UserStatusId không hợp lệ");
            }

            // Kiểm tra ClassId có tồn tại không (nếu có nhập)
            if (userRequest.ClassId.HasValue &&
                _classRepo.GetClassById(userRequest.ClassId.Value) == null)
            {
                return ApiResponse<UserResponse>.BadRequest("ClassId không hợp lệ");
            }
            // Kiểm tra mã người dùng đã tồn tại chưa
            if (_userRepo.GetUserByCode(userRequest.Code) != null)
            {
                return ApiResponse<UserResponse>.BadRequest("Mã người dùng đã tồn tại");
            }

            // Cập nhật thông tin người dùng
            user.FullName = userRequest.FullName;
            user.Email = userRequest.Email;
            user.Password = ComputeSha256(userRequest.Password);
            user.PhoneNumber = userRequest.PhoneNumber;
            user.Dob = userRequest.Dob;
            user.Gender = userRequest.Gender;
            user.AddressFull = userRequest.AddressFull;
            user.Street = userRequest.Street;
            user.RoleId = userRequest.RoleId;
            user.AcademicYearId = userRequest.AcademicYearId;
            user.ProvinceCode = userRequest.ProvinceCode;
            user.DistrictCode = userRequest.DistrictCode;
            user.WardCode = userRequest.WardCode;
            user.UserStatusId = userRequest.UserStatusId;
            user.ClassId = userRequest.ClassId;
            user.EntryType = userRequest.EntryType;
            user.Active = userRequest.Active;

            // Chỉ cập nhật mật khẩu nếu có nhập mới
            if (!string.IsNullOrEmpty(userRequest.Password))
            {
                user.Password = ComputeSha256(userRequest.Password);
            }

            try
            {
                var updatedUser = _userRepo.UpdateUser(user);
                return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(updatedUser));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.BadRequest(ex.Message);
            }
        }

        public ApiResponse<User> DeleteUser(int id)
        {
            var success = _userRepo.DeleteUser(id);
            return success
                ? ApiResponse<User>.Success()
                : ApiResponse<User>.NotFound("Không tìm thấy người dùng để xóa");
        }

        public ApiResponse<UserResponse> UpdateUserPassword(int userId, string newPassword)
        {
            var user = _userRepo.GetUserById(userId);
            if (user == null)
            {
                return ApiResponse<UserResponse>.NotFound("Không tìm thấy người dùng để cập nhật mật khẩu");
            }

            user.Password = ComputeSha256(newPassword);

            try
            {
                var updatedUser = _userRepo.UpdateUser(user);
                return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(updatedUser));
            }
            catch (Exception ex)
            {
                return ApiResponse<UserResponse>.BadRequest(ex.Message);
            }
        }


        public static string ComputeSha256(string? input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return null;
            }

            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input + "ledang"));
            StringBuilder builder = new();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }


    }
}
