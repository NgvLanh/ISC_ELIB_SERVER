using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly SubjectRepo _subjectRepo;
        private readonly SubjectGroupRepo _subjectGroupRepo;
        private readonly SubjectTypeRepo _subjectTypeRepo;
        private readonly IMapper _mapper;

        public SubjectService(SubjectRepo subjectRepo, IMapper mapper, SubjectGroupRepo subjectGroupRepo, SubjectTypeRepo subjectTypeRepo)
        {
            _subjectRepo = subjectRepo;
            _mapper = mapper;
            _subjectGroupRepo = subjectGroupRepo;
            _subjectTypeRepo = subjectTypeRepo;
        }

        public ApiResponse<ICollection<SubjectResponse>> GetSubject(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _subjectRepo.GetAllSubject().AsQueryable();

            query = query.Where(qr => qr.Active);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn?.ToLower() switch
            {
                "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                "code" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Code) : query.OrderBy(us => us.Code),
                "hourssemester1" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.HoursSemester1) : query.OrderBy(us => us.HoursSemester1),
                "hourssemester2" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.HoursSemester2) : query.OrderBy(us => us.HoursSemester2),
                _ => query.OrderBy(us => us.Id)
            };

            var total = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            var result = query.ToList();

            var response = _mapper.Map<ICollection<SubjectResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<SubjectResponse>>.Success(
                        data: response,
                        totalItems: total,
                        pageSize: pageSize,
                        page: page
                    )
                : ApiResponse<ICollection<SubjectResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SubjectResponse> GetSubjectById(long id)
        {
            var subject = _subjectRepo.GetSubjectById(id);
            return subject != null
                ? ApiResponse<SubjectResponse>.Success(_mapper.Map<SubjectResponse>(subject))
                : ApiResponse<SubjectResponse>.NotFound($"Không tìm thấy môn học có id {id}");
        }

        public ApiResponse<SubjectResponse> CreateSubject(SubjectRequest request)
        {
            var existing = _subjectRepo.GetAllSubject().FirstOrDefault(st => st.Name?.ToLower() == request.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<SubjectResponse>.Conflict("Tên môn học đã tồn tại");
            }
            var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(request.SubjectGroupId);
            if (subjectGroup == null) {
                return ApiResponse<SubjectResponse>.NotFound($"Tổ - bộ mộn có id {request.SubjectGroupId} không tồn tại");
            }
            var subjectType = _subjectTypeRepo.GetSubjectTypeById(request.SubjectTypeId);
            if (subjectType == null)
            {
                return ApiResponse<SubjectResponse>.NotFound($"Loại mộn học có id {request.SubjectTypeId} không tồn tại");
            }
            var create = _subjectRepo.CreateSubject(_mapper.Map<Subject>(request));
            return ApiResponse<SubjectResponse>.Success(_mapper.Map<SubjectResponse>(create));
        }
        public ApiResponse<SubjectResponse> UpdateSubject(long id, SubjectRequest request)
        {
            var subject = _subjectRepo.GetSubjectById(id);
            if (subject == null)
            {
                return ApiResponse<SubjectResponse>.NotFound($"Không tìm thấy môn học có id {id}");
            }
            var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(request.SubjectGroupId);
            if (subjectGroup == null)
            {
                return ApiResponse<SubjectResponse>.NotFound($"Tổ - bộ mộn có id {request.SubjectGroupId} không tồn tại");
            }
            var subjectType = _subjectTypeRepo.GetSubjectTypeById(request.SubjectTypeId);
            if (subjectType == null)
            {
                return ApiResponse<SubjectResponse>.NotFound($"Loại mộn học có id {request.SubjectTypeId} không tồn tại");
            }
            _mapper.Map(request, subject);
            var update = _subjectRepo.UpdateSubject(subject);
            return ApiResponse<SubjectResponse>.Success(_mapper.Map<SubjectResponse>(update));
        }

        public ApiResponse<string> DeleteSubject(long id)
        {
            var delete = _subjectRepo.DeleteSubject(id);
            if (delete)
            {
                return new ApiResponse<string>(0, "Xóa môn học thành công", null, null);
            }
            else
            {
                return ApiResponse<string>.NotFound($"Không tìm thấy môn học có id {id}");
            }
        }
    }
}
