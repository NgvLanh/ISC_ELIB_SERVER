using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{

    public class AcademicYearService : IAcademicYearService
    {
        private readonly AcademicYearRepo _repository;
        private readonly IMapper _mapper;

        public AcademicYearService(AcademicYearRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<AcademicYearResponse>> GetAcademicYears(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetAcademicYears().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(ay => ay.Id)
            };


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<AcademicYearResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<AcademicYearResponse>>
            .Success(response, page, pageSize, _repository.GetAcademicYears().Count)
             : ApiResponse<ICollection<AcademicYearResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<AcademicYearResponse> GetAcademicYearById(long id)
        {
            var academicYear = _repository.GetAcademicYearById(id);
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

            var newAcademicYear = new AcademicYear
            {
                StartTime = DateTime.SpecifyKind(academicYearRequest.StartTime, DateTimeKind.Unspecified),
                EndTime = DateTime.SpecifyKind(academicYearRequest.EndTime, DateTimeKind.Unspecified),
                SchoolId = academicYearRequest.SchoolId
            };
            try
            {

                var created = _repository.CreateAcademicYear(newAcademicYear);
                return ApiResponse<AcademicYearResponse>.Success(_mapper.Map<AcademicYearResponse>(created));
            }
            catch (Exception ex)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Mã trường không chính xác"); // dùng tạm 
            }
        }

        public ApiResponse<AcademicYearResponse> UpdateAcademicYear(long id, AcademicYearRequest academicYearRequest)
        {
            var existing = _repository.GetAcademicYearById(id);
            if (existing == null)
            {
                return ApiResponse<AcademicYearResponse>.NotFound($"Không tìm thấy năm học #{id}");
            }

            if (academicYearRequest.StartTime >= academicYearRequest.EndTime)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Ngày bắt đầu phải trước ngày kết thúc");
            }

            existing.StartTime = DateTime.SpecifyKind(academicYearRequest.StartTime, DateTimeKind.Unspecified);
            existing.EndTime = DateTime.SpecifyKind(academicYearRequest.EndTime, DateTimeKind.Unspecified);
            existing.SchoolId = academicYearRequest.SchoolId;
            try
            {


                var updated = _repository.UpdateAcademicYear(existing);
                return ApiResponse<AcademicYearResponse>.Success(_mapper.Map<AcademicYearResponse>(updated));
            }
            catch (Exception ex)
            {
                return ApiResponse<AcademicYearResponse>.BadRequest("Mã trường không chính xác"); // dùng tạm 
            }

        }

        public ApiResponse<object> DeleteAcademicYear(long id)
        {
            var success = _repository.DeleteAcademicYear(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy năm #{id} học để xóa");
        }
    }

}
