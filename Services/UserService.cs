using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IUserService
    {
        ApiResponse<ICollection<UserResponse>> GetUsers(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<UserResponse> GetUserById(long id);
        ApiResponse<UserResponse> GetUserByCode(string code);
        ApiResponse<UserResponse> CreateUser(UserRequest userRequest);
        ApiResponse<UserResponse> UpdateUser(UserRequest userRequest);
        ApiResponse<User> DeleteUser(long id);
    }

    public class UserService : IUserService
    {
        private readonly UserRepo _repository;
        private readonly IMapper _mapper;

        public UserService(UserRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<UserResponse>> GetUsers(int page, int pageSize, string search, string sortColumn, string sortOrder)
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

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<UserResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<UserResponse>>.Success(response)
                : ApiResponse<ICollection<UserResponse>>.NotFound("Không có dữ liệu người dùng");
        }

        public ApiResponse<UserResponse> GetUserById(long id)
        {
            var user = _repository.GetUserById(id);
            return user != null
                ? ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(user))
                : ApiResponse<UserResponse>.NotFound($"Không tìm thấy người dùng với ID #{id}");
        }

        public ApiResponse<UserResponse> GetUserByCode(string code)
        {
            var user = _repository.GetUsers().FirstOrDefault(u => u.Code?.ToLower() == code.ToLower());
            return user != null
                ? ApiResponse<UserResponse>.Success(_mapper.Map<UserResponse>(user))
                : ApiResponse<UserResponse>.NotFound($"Không tìm thấy người dùng với mã {code}");
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

        public ApiResponse<UserResponse> UpdateUser(UserRequest userRequest)
        {
            var user = _repository.GetUserById(userRequest.Id);
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

        public ApiResponse<User> DeleteUser(long id)
        {
            var success = _repository.DeleteUser(id);
            return success
                ? ApiResponse<User>.Success()
                : ApiResponse<User>.NotFound("Không tìm thấy người dùng để xóa");
        }
    }
}
