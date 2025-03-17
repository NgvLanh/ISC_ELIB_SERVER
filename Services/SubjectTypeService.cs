using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class SubjectTypeService : ISubjectTypeService
    {
        private readonly SubjectTypeRepo _subjectTypeRepo;
        private readonly IMapper _mapper;

        public SubjectTypeService(SubjectTypeRepo subjectTypeRepo, IMapper mapper)
        {
            _subjectTypeRepo = subjectTypeRepo;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<SubjectTypeResponse>> GetSubjectType(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _subjectTypeRepo.GetAllSubjectType().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                "Status" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Status) : query.OrderBy(us => us.Status),
                _ => query.OrderBy(us => us.Id)
            };
            query = query.Where(qr => qr.Active == true);

            var total = query.Count();

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<SubjectTypeResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<SubjectTypeResponse>>.Success(
                        data: response,
                        totalItems: total,
                        pageSize: pageSize,
                        page: page
                    )
                : ApiResponse<ICollection<SubjectTypeResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SubjectTypeResponse> GetSubjectTypeById(long id)
        {
            var subjectType = _subjectTypeRepo.GetSubjectTypeById(id);
            return subjectType != null 
                ? ApiResponse<SubjectTypeResponse>.Success(_mapper.Map<SubjectTypeResponse>(subjectType))
                : ApiResponse<SubjectTypeResponse>.NotFound($"Không tìm thấy loại môn học có id {id}");
        }


        public ApiResponse<SubjectTypeResponse> CreateSubjectType(SubjectTypeRequest subjectTypeRequest)
        {
            var existing = _subjectTypeRepo.GetAllSubjectType().FirstOrDefault(st => st.Name?.ToLower() == subjectTypeRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<SubjectTypeResponse>.Conflict("Tên loại môn học đã tồn tại");
            }
            var create = _subjectTypeRepo.CreateSubjectType(_mapper.Map<SubjectType>(subjectTypeRequest));
            return ApiResponse<SubjectTypeResponse>.Success(_mapper.Map<SubjectTypeResponse>(create));
        }

        public ApiResponse<SubjectTypeResponse> UpdateSubjectType(long id, SubjectTypeRequest subjectTypeRequest)
        {
            var subjectType = _subjectTypeRepo.GetSubjectTypeById(id);
            if (subjectType == null) {
                return ApiResponse<SubjectTypeResponse>.NotFound($"Không tìm thấy loại môn học có id {id}");
            }
            _mapper.Map(subjectTypeRequest, subjectType);
            var update = _subjectTypeRepo.UpdateSubjectType(subjectType);
            return ApiResponse<SubjectTypeResponse>.Success(_mapper.Map<SubjectTypeResponse>(update));
        }

        public ApiResponse<string> DeleteSubjectType(long id)
        {
            var delete = _subjectTypeRepo.DeleteSubjectType(id);
            if(delete)
            {
                return new ApiResponse<string>(0, "Xóa loại mộn học thành công", null, null);
            } else
            {
                return ApiResponse<string>.NotFound($"Không tìm thấy loại môn học có id {id}");
            }

        }
    }
}
