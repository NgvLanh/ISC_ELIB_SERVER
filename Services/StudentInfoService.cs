﻿using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public interface IStudentInfoService
    {
        ApiResponse<ICollection<StudentInfoResponses>> GetStudentInfos(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<StudentInfoResponses> GetStudentInfoById(long id);
        ApiResponse<StudentInfoResponses> CreateStudentInfo(StudentInfoRequest studentInfoRequest);
        ApiResponse<StudentInfoResponses> UpdateStudentInfo(StudentInfoRequest studentInfoRequest);
        ApiResponse<StudentInfoResponses> DeleteStudentInfo(long id);
    }

    public class StudentInfoService : IStudentInfoService
    {
        private readonly StudentInfoRepo _repository;
        private readonly IMapper _mapper;

        public StudentInfoService(StudentInfoRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<StudentInfoResponses>> GetStudentInfos(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAllStudentInfo().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    (s.GuardianName != null && s.GuardianName.ToLower().Contains(search.ToLower())) ||
                    (s.GuardianPhone != null && s.GuardianPhone.ToLower().Contains(search.ToLower()))
                );
            }

            query = sortColumn switch
            {
                "GuardianName" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.GuardianName) : query.OrderBy(s => s.GuardianName),
                "GuardianDob" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.GuardianDob) : query.OrderBy(s => s.GuardianDob),
                _ => sortOrder.ToLower() == "desc" ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<StudentInfoResponses>>(result);

            return result.Any()
                ? ApiResponse<ICollection<StudentInfoResponses>>.Success(response)
                : ApiResponse<ICollection<StudentInfoResponses>>.NotFound("Không có dữ liệu StudentInfo");
        }

        public ApiResponse<StudentInfoResponses> GetStudentInfoById(long id)
        {
            var studentInfo = _repository.GetStudentInfoById(id);
            return studentInfo != null
                ? ApiResponse<StudentInfoResponses>.Success(_mapper.Map<StudentInfoResponses>(studentInfo))
                : ApiResponse<StudentInfoResponses>.NotFound($"Không tìm thấy StudentInfo với ID #{id}");
        }

        public ApiResponse<StudentInfoResponses> CreateStudentInfo(StudentInfoRequest studentInfoRequest)
        {
            var studentInfo = _mapper.Map<StudentInfo>(studentInfoRequest);
            _repository.AddStudentInfo(studentInfo);
            var createdStudentInfo = _mapper.Map<StudentInfoResponses>(studentInfo);

            return ApiResponse<StudentInfoResponses>.Success(createdStudentInfo, "Tạo StudentInfo thành công");
        }

        public ApiResponse<StudentInfoResponses> UpdateStudentInfo(StudentInfoRequest studentInfoRequest)
        {
            var studentInfo = _repository.GetStudentInfoById(studentInfoRequest.Id);
            if (studentInfo == null)
            {
                return ApiResponse<StudentInfoResponses>.NotFound("Không tìm thấy StudentInfo để cập nhật");
            }

            _mapper.Map(studentInfoRequest, studentInfo);
            _repository.UpdateStudentInfo(studentInfo);
            var updatedStudentInfo = _mapper.Map<StudentInfoResponses>(studentInfo);

            return ApiResponse<StudentInfoResponses>.Success(updatedStudentInfo, "Cập nhật StudentInfo thành công");
        }

        public ApiResponse<StudentInfoResponses> DeleteStudentInfo(long id)
        {
            var studentInfo = _repository.GetStudentInfoById(id);
            if (studentInfo == null)
            {
                return ApiResponse<StudentInfoResponses>.NotFound("Không tìm thấy StudentInfo để xóa");
            }

            _repository.DeleteStudentInfo(id);
            return ApiResponse<StudentInfoResponses>.Success(null, "Xóa StudentInfo thành công");
        }
    }
}
