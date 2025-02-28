using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{

    public class EducationLevelService : IEducationLevelService
    {
        private readonly EducationLevelRepo _repository;
        private readonly IMapper _mapper;

        public EducationLevelService(EducationLevelRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<EducationLevelResponse>> GetEducationLevels(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetEducationLevels().AsQueryable();

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

            var response = _mapper.Map<ICollection<EducationLevelResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<EducationLevelResponse>>.Success(response) : ApiResponse<ICollection<EducationLevelResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<EducationLevelResponse> GetEducationLevelById(long id)
        {
            var EducationLevel = _repository.GetEducationLevelById(id);
            return EducationLevel != null
                ? ApiResponse<EducationLevelResponse>.Success(_mapper.Map<EducationLevelResponse>(EducationLevel))
                : ApiResponse<EducationLevelResponse>.NotFound($"Không tìm thấy cấp bậc đào tạo #{id}");
        }

        public ApiResponse<EducationLevelResponse> CreateEducationLevel(EducationLevelRequest EducationLevelRequest)
        {
            var ListEducationLevel = _repository.GetEducationLevels().Where(item => item.Active);

            if(ListEducationLevel.Any(item => item.Name.Equals(EducationLevelRequest.Name))){
                return ApiResponse<EducationLevelResponse>.BadRequest("Tên cấp bậc đào tạo đã tồn tại");
            }

            var newEducationLevel = new EducationLevel
            {
                Name = EducationLevelRequest.Name,
                Status = EducationLevelRequest.Status,
                Description = EducationLevelRequest.Description
            };
            try
            {
                var created = _repository.CreateEducationLevel(newEducationLevel);
                return ApiResponse<EducationLevelResponse>.Success(_mapper.Map<EducationLevelResponse>(created));
            }
            catch (Exception ex)
            {
                return ApiResponse<EducationLevelResponse>.BadRequest("Lỗi....");
            }
        }

        public ApiResponse<EducationLevelResponse> UpdateEducationLevel(long id, EducationLevelRequest EducationLevelRequest)
        {
            var existing = _repository.GetEducationLevelById(id);
            if (existing == null)
            {
                return ApiResponse<EducationLevelResponse>.NotFound($"Không tìm thấy cấp bậc đào tạo #{id}");
            }

            var ListEducationLevel = _repository.GetEducationLevels().Where(item => item.Active); ;

            if (ListEducationLevel.Any(item => item.Name.Equals(EducationLevelRequest.Name) && item.Id != id))
            {
                ApiResponse<EducationLevelResponse>.BadRequest("Tên cấp bậc đào tạo đã tồn tại");
            }

            existing.Name = EducationLevelRequest.Name;
            existing.Status = EducationLevelRequest.Status;
            existing.Description = EducationLevelRequest.Description;
            try
            {


                var updated = _repository.UpdateEducationLevel(existing);
                return ApiResponse<EducationLevelResponse>.Success(_mapper.Map<EducationLevelResponse>(updated));
            }
            catch (Exception ex)
            {
                return ApiResponse<EducationLevelResponse>.BadRequest("Lỗi...."); 
            }

        }

        public ApiResponse<object> DeleteEducationLevel(long id)
        {
            var success = _repository.DeleteEducationLevel(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy cập bậc đào tạo #{id} học để xóa");
        }
    }

}
