using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Linq;

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

        public ApiResponse<ICollection<SubjectTypeResponse>> GetSubjectType(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder, string? date)
        {
            var query = _subjectTypeRepo.GetAllSubjectType().AsQueryable();

            query = query.Where(qr => qr.Active);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn?.ToLower() switch
            {
                "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                "status" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Status) : query.OrderBy(us => us.Status),
                _ => query.OrderBy(us => us.Id)
            };
            query = query.Where(qr => qr.Active == true);

            if (date != null)
            {
                DateTime dateValue;
                var check = DateTime.TryParse(date, out dateValue);
                if (check)
                {
                    query = query.Where(qr => qr.Date.HasValue && qr.Date.Value.Date.Year == dateValue.Date.Year);
                }
                else
                {
                    return ApiResponse<ICollection<SubjectTypeResponse>>.BadRequest($"Date không đúng định dạng!!!");
                }
            }

            var total = query.Count();

            if (page.HasValue && pageSize.HasValue) {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

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
            var subjectType = _mapper.Map<SubjectType>(subjectTypeRequest);
            subjectType.Date = DateTime.Now;

            var create = _subjectTypeRepo.CreateSubjectType(subjectType);
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
