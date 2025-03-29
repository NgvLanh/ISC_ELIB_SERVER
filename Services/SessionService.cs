﻿using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class SessionService : ISessionService
    {

        private readonly SessionRepo _sessionRepo;


        private readonly IMapper _mapper;

        public SessionService(SessionRepo sessionRepo, IMapper mapper)
        {

            _sessionRepo = sessionRepo;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<SessionResponse>> GetSessions(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _sessionRepo.GetAllSessions().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id),
                _ => query.OrderBy(s => s.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<SessionResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<SessionResponse>>.Success(response, page, pageSize, _sessionRepo.Count())
                : ApiResponse<ICollection<SessionResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SessionResponse> GetSessionById(int id)
        {
            var session = _sessionRepo.GetSessionById(id);
            return session != null
                ? ApiResponse<SessionResponse>.Success(_mapper.Map<SessionResponse>(session))
                : ApiResponse<SessionResponse>.NotFound($"Không tìm thấy Session có id {id}");
        }

        public ApiResponse<SessionResponse> CreateSession(SessionRequest request)
        {
            if (request.StartDate == null || request.EndDate == null)
            {
                return ApiResponse<SessionResponse>.BadRequest("Ngày bắt đầu và ngày kết thúc không được để trống.");
            }

            if (request.StartDate.Value.Date >= request.EndDate.Value.Date)
            {
                return ApiResponse<SessionResponse>.BadRequest("Ngày kết thúc phải sau ngày bắt đầu ít nhất một ngày.");
            }

            //if (request.ExamId == null || request.TeachingAssignmentId == null)
            //{
            //    return ApiResponse<SessionResponse>.BadRequest("ExamId và TeachingAssignmentId không được để trống.");
            //}

            var session = _mapper.Map<Session>(request);
            session.Password = PasswordHasher.HashPassword(request.Password);
            // Chuyển đổi DateTime về Unspecified để tránh lỗi với PostgreSQL
            session.StartDate = DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Unspecified);
            session.EndDate = DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Unspecified);

            var created = _sessionRepo.CreateSession(session);
            return ApiResponse<SessionResponse>.Success(_mapper.Map<SessionResponse>(created));
        }

        public ApiResponse<SessionResponse> UpdateSession(int id, SessionRequest request)
        {
            var session = _sessionRepo.GetSessionById(id);
            if (session == null)
            {
                return ApiResponse<SessionResponse>.NotFound($"Không tìm thấy Session có id {id}");
            }

            if (request.StartDate == null || request.EndDate == null)
            {
                return ApiResponse<SessionResponse>.BadRequest("Ngày bắt đầu và ngày kết thúc không được để trống.");
            }

            if (request.StartDate.Value.Date >= request.EndDate.Value.Date)
            {
                return ApiResponse<SessionResponse>.BadRequest("Ngày kết thúc phải sau ngày bắt đầu ít nhất một ngày.");
            }

            _mapper.Map(request, session);

            // Nếu có cập nhật mật khẩu thì mã hóa lại
            if (!string.IsNullOrEmpty(request.Password))
            {
                session.Password = PasswordHasher.HashPassword(request.Password);
            }

            session.StartDate = DateTime.SpecifyKind(request.StartDate.Value, DateTimeKind.Unspecified);
            session.EndDate = DateTime.SpecifyKind(request.EndDate.Value, DateTimeKind.Unspecified);

            var updated = _sessionRepo.UpdateSession(session);
            return ApiResponse<SessionResponse>.Success(_mapper.Map<SessionResponse>(updated));
        }



        public ApiResponse<string> DeleteSession(int id)
        {
            var deleted = _sessionRepo.DeleteSession(id);
            return deleted
                ? ApiResponse<string>.Success("Xóa Session thành công")
                : ApiResponse<string>.NotFound($"Không tìm thấy Session có id {id}");
        }


        public ApiResponse<SessionResponse> JoinSession(JoinSessionRequest request)
        {
            Console.WriteLine($"Received Request: ShareCodeUrl={request.ShareCodeOrSessionId}, Password={request.Password}");

            if (string.IsNullOrEmpty(request.ShareCodeOrSessionId) || string.IsNullOrEmpty(request.Password))
            {
                return ApiResponse<SessionResponse>.BadRequest("Vui lòng cung cấp ShareCodeUrl hoặc Id và Password.");
            }

            var session = _sessionRepo.GetSessionByJoinInfo(request);
            if (session == null)
            {
                return ApiResponse<SessionResponse>.NotFound("Lớp học không tồn tại hoặc thông tin không chính xác.");
            }

            // Hash mật khẩu nhập vào và so sánh với mật khẩu đã lưu
            // string hashedInputPassword = PasswordHasher.HashPassword(request.Password);
            //  Console.WriteLine($"Received Request: Password={hashedInputPassword}");
            // if (session.Password != hashedInputPassword)
            // {
            //     return ApiResponse<SessionResponse>.Unauthorized("Mật khẩu không chính xác.");
            // }

            var response = _mapper.Map<SessionResponse>(session);
            return ApiResponse<SessionResponse>.Success();
        }

        public ApiResponse<ICollection<SessionStudentResponse>> GetFilteredSessions(int userId,int page, int pageSize, SessionStudentFilterRequest request)
        {
            var query = _sessionRepo.GetFilteredSessions(userId,request);
            
            int totalItems = query.Count(); // Đếm số lượng bản ghi thực tế sau khi lọc
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Tính số trang đúng

            // query = query.sortColumn switch
            // {
            //     "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
            //     "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id),
            //     _ => query.OrderBy(s => s.Id)
            // };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<SessionStudentResponse>>(result);

            return result.Any()
               ? ApiResponse<ICollection<SessionStudentResponse>>.Success(response, page, pageSize, totalItems)
                // ? ApiResponse<ICollection<SessionStudentResponse>>.Success(response, page, pageSize, _sessionRepo.Count())
                : ApiResponse<ICollection<SessionStudentResponse>>.NotFound("Không có dữ liệu");
            // return ApiResponse<ICollection<SessionStudentResponse>>.Success(response);
        }
    }

}


