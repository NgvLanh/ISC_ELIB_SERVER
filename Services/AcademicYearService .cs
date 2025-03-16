﻿using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{

    public class AcademicYearService : IAcademicYearService
    {
        private readonly AcademicYearRepo _academicYearRepo;
        private readonly SchoolRepo _schoolRepo;
        private readonly IMapper _mapper;

        public AcademicYearService(AcademicYearRepo academicYearRepo, SchoolRepo schoolRepo, IMapper mapper)
        {
            _academicYearRepo = academicYearRepo;
            _schoolRepo = schoolRepo;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<AcademicYearResponse>> GetAcademicYears(
        int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _academicYearRepo.GetAcademicYears().AsQueryable();

            query = sortColumn?.ToLower() switch
            {
                "id" when sortOrder?.ToLower() == "desc" => query.OrderByDescending(ay => ay.Id),
                "id" => query.OrderBy(ay => ay.Id),
                _ => query.OrderBy(ay => ay.Id)
            };

            int totalRecords = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<AcademicYearResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<AcademicYearResponse>>.Success(response, page, pageSize, totalRecords)
                : ApiResponse<ICollection<AcademicYearResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<AcademicYearResponse> GetAcademicYearById(long id)
        {
            var academicYear = _academicYearRepo.GetAcademicYearById(id);
            return academicYear != null
                ? ApiResponse<AcademicYearResponse>.Success(_mapper.Map<AcademicYearResponse>(academicYear))
                : ApiResponse<AcademicYearResponse>.NotFound($"Không tìm thấy năm học #{id}");
        }

        public ApiResponse<AcademicYearResponse> CreateAcademicYear(AcademicYearRequest academicYearRequest)
        {
            if (academicYearRequest.StartTime >= academicYearRequest.EndTime)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }

            var duration = (academicYearRequest.EndTime - academicYearRequest.StartTime).TotalDays / 365;

            if (duration < 1 || duration > 5)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Niên khóa phải kéo dài ít nhất 1 năm và nhiều nhất 5 năm");
            }
            var school = _schoolRepo.GetSchoolById((long)academicYearRequest.SchoolId);

            if (school == null)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Mã trường không chính xác");
            }

            var newAcademicYear = new AcademicYear
            {
                StartTime = DateTime.SpecifyKind(academicYearRequest.StartTime, DateTimeKind.Unspecified),
                EndTime = DateTime.SpecifyKind(academicYearRequest.EndTime, DateTimeKind.Unspecified),
                SchoolId = academicYearRequest.SchoolId
            };
            try
            {
                var created = _academicYearRepo.CreateAcademicYear(newAcademicYear);
                return ApiResponse<AcademicYearResponse>.Success(_mapper.Map<AcademicYearResponse>(created));
            }
            catch (Exception ex)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest(ex.Message);
            }
        }

        public ApiResponse<AcademicYearResponse> UpdateAcademicYear(long id, AcademicYearRequest academicYearRequest)
        {
            var existing = _academicYearRepo.GetAcademicYearById(id);
            if (existing == null)
            {
                return ApiResponse<AcademicYearResponse>.NotFound($"Không tìm thấy năm học #{id}");
            }

            if (academicYearRequest.StartTime >= academicYearRequest.EndTime)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }

            var duration = (academicYearRequest.EndTime - academicYearRequest.StartTime).TotalDays / 365;

            if (duration < 1 || duration > 5)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Niên khóa phải kéo dài ít nhất 1 năm và nhiều nhất 5 năm");
            }
            var school = _schoolRepo.GetSchoolById((long)academicYearRequest.SchoolId);

            if (school == null)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Mã trường không chính xác");
            }

            existing.StartTime = DateTime.SpecifyKind(academicYearRequest.StartTime, DateTimeKind.Unspecified);
            existing.EndTime = DateTime.SpecifyKind(academicYearRequest.EndTime, DateTimeKind.Unspecified);
            existing.SchoolId = academicYearRequest.SchoolId;
            try
            {
                var updated = _academicYearRepo.UpdateAcademicYear(existing);
                return ApiResponse<AcademicYearResponse>.Success(_mapper.Map<AcademicYearResponse>(updated));
            }
            catch (Exception ex)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest(ex.Message);
            }

        }

        public ApiResponse<object> DeleteAcademicYear(long id)
        {
            var success = _academicYearRepo.DeleteAcademicYear(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy năm #{id} học để xóa");
        }
    }

}
