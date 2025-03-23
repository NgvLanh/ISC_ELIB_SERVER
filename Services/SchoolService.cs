using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Utils;

namespace ISC_ELIB_SERVER.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly SchoolRepo _repository;
        private readonly IMapper _mapper;

        public SchoolService(SchoolRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<SchoolResponse>> GetSchools(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetSchools().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Name.Contains(search));
            }

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(s => s.Id)
            };

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<SchoolResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<SchoolResponse>>
            .Success(response, page, pageSize, _repository.GetSchools().Count)
            : ApiResponse<ICollection<SchoolResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SchoolResponse> GetSchoolById(long id)
        {
            var school = _repository.GetSchoolById(id);
            return school != null
                ? ApiResponse<SchoolResponse>.Success(_mapper.Map<SchoolResponse>(school))
                : ApiResponse<SchoolResponse>.NotFound($"Không tìm thấy trường #{id}");
        }

        public ApiResponse<SchoolResponse> CreateSchool(SchoolRequest schoolRequest)
        {
            string trainingModel = schoolRequest.TrainingModel.ToLower().Replace(" ", "");

            if (!Enum.IsDefined(typeof(TrainingModel), trainingModel))
            {
                return ApiResponse<SchoolResponse>.BadRequest("Mô hình đào tạo không hợp lệ, chỉ chấp nhận 'Công lập' hoặc 'Dân lập'" + trainingModel);
            }

            if (_repository.IsSchoolNameExists(schoolRequest.Name))
            {
                return ApiResponse<SchoolResponse>.BadRequest("Tên trường đã tồn tại");
            }

            var newSchool = new School
            {
                Code = schoolRequest.Code,
                Name = schoolRequest.Name,
                ProvinceId = schoolRequest.ProvinceId,
                DistrictId = schoolRequest.DistrictId,
                WardId = schoolRequest.WardId,
                HeadOffice = schoolRequest.HeadOffice,
                SchoolType = schoolRequest.SchoolType,
                PhoneNumber = schoolRequest.PhoneNumber,
                Email = schoolRequest.Email,
                EstablishedDate = DateTime.SpecifyKind(schoolRequest.EstablishedDate, DateTimeKind.Unspecified),
                TrainingModel = EnumUtil.GetEnumStringValue<TrainingModel>(trainingModel),
                WebsiteUrl = schoolRequest.WebsiteUrl,
                UserId = schoolRequest.UserId,
                EducationLevelId = schoolRequest.EducationLevelId
            };

            try
            {
                var created = _repository.CreateSchool(newSchool);
                return ApiResponse<SchoolResponse>.Success(_mapper.Map<SchoolResponse>(created));
            }
            catch (Exception)
            {
                return ApiResponse<SchoolResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
        }

        public ApiResponse<SchoolResponse> UpdateSchool(long id, SchoolRequest schoolRequest)
        {
            var existing = _repository.GetSchoolById(id);
            if (existing == null)
            {
                return ApiResponse<SchoolResponse>.NotFound($"Không tìm thấy trường #{id}");
            }
            string trainingModel = schoolRequest.TrainingModel.ToLower().Replace(" ", "");

            if (!Enum.IsDefined(typeof(TrainingModel), trainingModel))
            {
                return ApiResponse<SchoolResponse>.BadRequest("Mô hình đào tạo không hợp lệ, chỉ chấp nhận 'Công lập' hoặc 'Dân lập'");
            }

            if (_repository.IsSchoolNameExists(schoolRequest.Name, id))
            {
                return ApiResponse<SchoolResponse>.BadRequest("Tên trường đã tồn tại");
            }

            existing.Code = schoolRequest.Code;
            existing.Name = schoolRequest.Name;
            existing.ProvinceId = schoolRequest.ProvinceId;
            existing.DistrictId = schoolRequest.DistrictId;
            existing.WardId = schoolRequest.WardId;
            existing.HeadOffice = schoolRequest.HeadOffice;
            existing.SchoolType = schoolRequest.SchoolType;
            existing.PhoneNumber = schoolRequest.PhoneNumber;
            existing.Email = schoolRequest.Email;
            existing.EstablishedDate = DateTime.SpecifyKind(schoolRequest.EstablishedDate, DateTimeKind.Unspecified);
            existing.TrainingModel = EnumUtil.GetEnumStringValue<TrainingModel>(trainingModel);
            existing.WebsiteUrl = schoolRequest.WebsiteUrl;
            existing.UserId = schoolRequest.UserId;
            existing.EducationLevelId = schoolRequest.EducationLevelId;

            try
            {
                var updated = _repository.UpdateSchool(existing);
                return ApiResponse<SchoolResponse>.Success(_mapper.Map<SchoolResponse>(updated));
            }
            catch (Exception)
            {
                return ApiResponse<SchoolResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
        }

        public ApiResponse<object> DeleteSchool(long id)
        {
            var success = _repository.DeleteSchool(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy trường #{id} để xóa");
        }
    }
}
