﻿using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface IUserStatusService
    {
        ApiResponse<ICollection<UserStatusResponse>> GetUserStatuses(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<UserStatusResponse> GetUserStatusById(long id);
        ApiResponse<UserStatusResponse> GetUserStatusByName(string name);
        ApiResponse<UserStatusResponse> CreateUserStatus(UserStatusRequest userStatusRequest);
        ApiResponse<UserStatus> UpdateUserStatus(UserStatus userStatus);
        ApiResponse<UserStatus> DeleteUserStatus(long id);
    }


    public class UserStatusService : IUserStatusService
    {
        private readonly UserStatusRepo _repository;
        private readonly IMapper _mapper;

        public UserStatusService(UserStatusRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<UserStatusResponse>> GetUserStatuses(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetUserStatuses().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<UserStatusResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<UserStatusResponse>>.Success(response)
                : ApiResponse<ICollection<UserStatusResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<UserStatusResponse> GetUserStatusById(long id)
        {
            var userStatus = _repository.GetUserStatusById(id);
            return userStatus != null
                ? ApiResponse<UserStatusResponse>.Success(_mapper.Map<UserStatusResponse>(userStatus))
                : ApiResponse<UserStatusResponse>.NotFound($"Không tìm thấy trạng thái người dùng #{id}");
        }

        public ApiResponse<UserStatusResponse> GetUserStatusByName(string name)
        {
            var userStatus = _repository.GetUserStatuses().FirstOrDefault(us => us.Name == name);
            return userStatus != null
                ? ApiResponse<UserStatusResponse>.Success(_mapper.Map<UserStatusResponse>(userStatus))
                : ApiResponse<UserStatusResponse>.NotFound($"Không tìm thấy trạng thái người dùng có tên: {name}");
        }

        public ApiResponse<UserStatusResponse> CreateUserStatus(UserStatusRequest userStatusRequest)
        {
            var existing = _repository.GetUserStatuses().FirstOrDefault(us => us.Name == userStatusRequest.Name);
            if (existing != null)
            {
                return ApiResponse<UserStatusResponse>.Conflict("Tên trạng thái đã tồn tại");
            }

            var created = _repository.CreateUserStatus(new UserStatus() { Name = userStatusRequest.Name });
            return ApiResponse<UserStatusResponse>.Success(_mapper.Map<UserStatusResponse>(created));
        }

        public ApiResponse<UserStatus> UpdateUserStatus(UserStatus userStatus)
        {
            var updated = _repository.UpdateUserStatus(userStatus);
            return updated != null
                ? ApiResponse<UserStatus>.Success(updated)
                : ApiResponse<UserStatus>.NotFound("Không tìm thấy trạng thái người dùng để cập nhật");
        }

        public ApiResponse<UserStatus> DeleteUserStatus(long id)
        {
            var success = _repository.DeleteUserStatus(id);
            return success
                ? ApiResponse<UserStatus>.Success()
                : ApiResponse<UserStatus>.NotFound("Không tìm thấy trạng thái người dùng để xóa");
        }
    }

}
