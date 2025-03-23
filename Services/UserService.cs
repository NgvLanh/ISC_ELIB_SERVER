using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepo _repository;
        private readonly IMapper _mapper;
        private readonly GhnService _ghnService;

        public UserService(UserRepo userRepo, RoleRepo roleRepo, AcademicYearRepo academicYearRepo,
            UserStatusRepo userStatusRepo, ClassRepo classRepo, IMapper mapper, GhnService ghnService)
        {
            _repository = repository;
            _mapper = mapper;
            _ghnService = ghnService;
        }

        public async Task<ApiResponse<ICollection<UserResponse>>> GetUsers(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetUsers().AsQueryable();

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

            return ApiResponse<UserResponse>.Success(response);
        }

        public ApiResponse<UserResponse> CreateUser(UserRequest userRequest)
        {
            // Kiểm tra nếu người dùng với mã hoặc email đã tồn tại
            var existing = _repository.GetUsers().FirstOrDefault(u => u.Code?.ToLower() == userRequest.Code?.ToLower() || u.Email?.ToLower() == userRequest.Email?.ToLower());
            if (existing != null)
            {
                return ApiResponse<UserResponse>.Conflict("Mã người dùng hoặc email đã tồn tại");
            }

            var user = new User
            {
                Code = userRequest.Code,
                FullName = userRequest.FullName,
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                Dob = userRequest.Dob,
                Gender = userRequest.Gender,
                AddressFull = userRequest.AddressFull,
                ProvinceCode = userRequest.ProvinceCode,
                DistrictCode = userRequest.DistrictCode,
                WardCode = userRequest.WardCode,
                Street = userRequest.Street,
                RoleId = userRequest.RoleId,
                AcademicYearId = userRequest.AcademicYearId,
                UserStatusId = userRequest.UserStatusId,
                ClassId = userRequest.ClassId,
                EntryType = userRequest.EntryType
            };

            var createdUser = _repository.CreateUser(user);
            return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(createdUser));
        }

        public ApiResponse<UserResponse> UpdateUser(int id, UserRequest userRequest)
        {
            var user = _repository.GetUserById(id);
            if (user == null)
            {
                return ApiResponse<UserResponse>.NotFound("Không tìm thấy người dùng để cập nhật");
            }

            // Cập nhật thông tin người dùng
            user.FullName = userRequest.FullName;
            user.Email = userRequest.Email;
            user.PhoneNumber = userRequest.PhoneNumber;
            user.Dob = userRequest.Dob;
            user.Gender = userRequest.Gender;
            user.AddressFull = userRequest.AddressFull;
            user.ProvinceCode = userRequest.ProvinceCode;
            user.DistrictCode = userRequest.DistrictCode;
            user.WardCode = userRequest.WardCode;
            user.Street = userRequest.Street;
            user.RoleId = userRequest.RoleId;
            user.AcademicYearId = userRequest.AcademicYearId;
            user.UserStatusId = userRequest.UserStatusId;
            user.ClassId = userRequest.ClassId;
            user.EntryType = userRequest.EntryType;

            var updatedUser = _repository.UpdateUser(user);
            return ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(updatedUser));
        }

        public ApiResponse<User> DeleteUser(int id)
        {
            var success = _repository.DeleteUser(id);
            return success
                ? ApiResponse<User>.Success()
                : ApiResponse<User>.NotFound("Không tìm thấy người dùng để xóa");
        }
    }
}
