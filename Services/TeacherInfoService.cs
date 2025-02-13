using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public interface ITeacherInfoService
    {
        ApiResponse<ICollection<TeacherInfoResponses>> GetTeacherInfos(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TeacherInfoResponses> GetTeacherInfoById(long id);
        ApiResponse<TeacherInfoResponses> GetTeacherInfoByCode(string code);
        ApiResponse<TeacherInfoResponses> CreateTeacherInfo(TeacherInfoRequest teacherInfoRequest);
        ApiResponse<TeacherInfoResponses> UpdateTeacherInfo(TeacherInfoRequest teacherInfoRequest);
        ApiResponse<TeacherInfoResponses> DeleteTeacherInfo(long id);
    }

    public class TeacherInfoService : ITeacherInfoService
    {
        private readonly TeacherInfoRepo _repository;
        private readonly IMapper _mapper;

        public TeacherInfoService(TeacherInfoRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TeacherInfoResponses>> GetTeacherInfos(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAllTeacherInfo().AsQueryable();

            // Tìm kiếm theo một số thuộc tính ví dụ: CCCD và AddressFull
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t =>
                    (t.Cccd != null && t.Cccd.ToLower().Contains(search.ToLower())) ||
                    (t.AddressFull != null && t.AddressFull.ToLower().Contains(search.ToLower()))
                );
            }

            // Sắp xếp theo các cột được chỉ định
            query = sortColumn switch
            {
                "Cccd" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Cccd) : query.OrderBy(t => t.Cccd),
                "IssuedDate" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.IssuedDate) : query.OrderBy(t => t.IssuedDate),
                _ => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<TeacherInfoResponses>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TeacherInfoResponses>>.Success(response)
                : ApiResponse<ICollection<TeacherInfoResponses>>.NotFound("Không có dữ liệu TeacherInfo");
        }

        public ApiResponse<TeacherInfoResponses> GetTeacherInfoById(long id)
        {
            var teacherInfo = _repository.GetTeacherInfoById(id);
            return teacherInfo != null
                ? ApiResponse<TeacherInfoResponses>.Success(_mapper.Map<TeacherInfoResponses>(teacherInfo))
                : ApiResponse<TeacherInfoResponses>.NotFound($"Không tìm thấy TeacherInfo với ID #{id}");
        }

        public ApiResponse<TeacherInfoResponses> GetTeacherInfoByCode(string code)
        {
            // Giả sử thuộc tính CCCD được dùng làm mã code
            var teacherInfo = _repository.GetAllTeacherInfo()
                                         .FirstOrDefault(t => t.Cccd != null && t.Cccd.ToLower() == code.ToLower());
            return teacherInfo != null
                ? ApiResponse<TeacherInfoResponses>.Success(_mapper.Map<TeacherInfoResponses>(teacherInfo))
                : ApiResponse<TeacherInfoResponses>.NotFound($"Không tìm thấy TeacherInfo với mã {code}");
        }

        public ApiResponse<TeacherInfoResponses> CreateTeacherInfo(TeacherInfoRequest teacherInfoRequest)
        {
            // Kiểm tra xem TeacherInfo với mã (CCCD) đã tồn tại hay chưa
            var existing = _repository.GetAllTeacherInfo()
                                      .FirstOrDefault(t => t.Cccd != null && t.Cccd.ToLower() == teacherInfoRequest.Cccd?.ToLower());
            if (existing != null)
            {
                return ApiResponse<TeacherInfoResponses>.Conflict("TeacherInfo với mã này đã tồn tại");
            }

            var teacherInfo = _mapper.Map<TeacherInfo>(teacherInfoRequest);
            _repository.AddTeacherInfo(teacherInfo);
            var createdTeacherInfo = _mapper.Map<TeacherInfoResponses>(teacherInfo);

            return ApiResponse<TeacherInfoResponses>.Success(createdTeacherInfo, "Tạo TeacherInfo thành công");
        }

        public ApiResponse<TeacherInfoResponses> UpdateTeacherInfo(TeacherInfoRequest teacherInfoRequest)
        {
            var teacherInfo = _repository.GetTeacherInfoById(teacherInfoRequest.Id);
            if (teacherInfo == null)
            {
                return ApiResponse<TeacherInfoResponses>.NotFound("Không tìm thấy TeacherInfo để cập nhật");
            }

            _mapper.Map(teacherInfoRequest, teacherInfo);
            _repository.UpdateTeacherInfo(teacherInfo);
            var updatedTeacherInfo = _mapper.Map<TeacherInfoResponses>(teacherInfo);

            return ApiResponse<TeacherInfoResponses>.Success(updatedTeacherInfo, "Cập nhật TeacherInfo thành công");
        }

        public ApiResponse<TeacherInfoResponses> DeleteTeacherInfo(long id)
        {
            var teacherInfo = _repository.GetTeacherInfoById(id);
            if (teacherInfo == null)
            {
                return ApiResponse<TeacherInfoResponses>.NotFound("Không tìm thấy TeacherInfo để xóa");
            }

            _repository.DeleteTeacherInfo(id);
            return ApiResponse<TeacherInfoResponses>.Success(null, "Xóa TeacherInfo thành công");
        }
    }
}
