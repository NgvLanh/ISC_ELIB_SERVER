using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{

    public class SubjectGroupService : ISubjectGroupService
    {
        private readonly SubjectGroupRepo _subjectGroupRepo;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;
        public SubjectGroupService(SubjectGroupRepo subjectGroupRepo, IMapper mapper, isc_dbContext context)
        {
            _subjectGroupRepo = subjectGroupRepo;
            _mapper = mapper;
            _context = context;
        }

        public ApiResponse<ICollection<SubjectGroupResponse>> GetSubjectGroup(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _subjectGroupRepo.GetAllSubjectGroup().AsQueryable();

            query = query.Where(qr => qr.Active);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn?.ToLower() switch
            {
                "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "id" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };
            query = query.Where(qr => qr.Active == true);

            var total = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<SubjectGroupResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<SubjectGroupResponse>>.Success(
                        data: response,
                        totalItems: total,
                        pageSize: pageSize,
                        page: page
                    )
                : ApiResponse<ICollection<SubjectGroupResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<SubjectGroupResponse> GetSubjectGroupById(long id)
        {
            var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(id);
            return subjectGroup != null
                ? ApiResponse<SubjectGroupResponse>.Success(_mapper.Map<SubjectGroupResponse>(subjectGroup))
                : ApiResponse<SubjectGroupResponse>.NotFound($"Không tìm thấy tổ - bộ môn có id {id}");
        }

        public ApiResponse<SubjectGroupResponse> CreateSubjectGroup(SubjectGroupRequest request)
        {
           

            var existing = _subjectGroupRepo.GetAllSubjectGroup().FirstOrDefault(st => st.Name?.ToLower() == request.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<SubjectGroupResponse>.Conflict("Tên tổ - bộ môn đã tồn tại");
            }
            var teacher = _context.Users.Where(u => u.Role.Name.ToLower() == "teacher").FirstOrDefault(x => x.Id == request.TeacherId);
            if (teacher == null)
            {
                return ApiResponse<SubjectGroupResponse>.NotFound($"Teacher có id {request.TeacherId} không tồn tại");
            }

            if (request?.subjectId == null || request.subjectId.Any(id => id.GetType() != typeof(int)))
            {
                return ApiResponse<SubjectGroupResponse>.Fail("Vui lòng truyền vào mảng là số nguyên!!!");
            }

            List<Subject> listSubjects = new List<Subject>();
            foreach (var id in request.subjectId)
            {
                var subject = _context.Subjects.FirstOrDefault(x => x.Id == id);
                if(subject == null)
                {
                    return ApiResponse<SubjectGroupResponse>.NotFound($"Môn học có id {id} không tồn tại");
                } 
                else
                {
                    listSubjects.Add(subject);
                }
            }
            var create = _subjectGroupRepo.CreateSubjectGroup(_mapper.Map<SubjectGroup>(request));
            
            List<SubjectSubjectGroup> subjectSubjectGroups = new List<SubjectSubjectGroup>();
            
            foreach (var subject in listSubjects) {
                subjectSubjectGroups.Add(new SubjectSubjectGroup
                {
                    SubjectId = subject.Id,
                    SubjectGroupId = create.Id
                });
            }

            _context.AddRange(subjectSubjectGroups);
            _context.SaveChanges();

            return ApiResponse<SubjectGroupResponse>.Success(_mapper.Map<SubjectGroupResponse>(create));
        }

        public ApiResponse<SubjectGroupResponse> UpdateSubjectGroup(long id, SubjectGroupRequest request)
        {
            var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(id);
            if (subjectGroup == null)
            {
                return ApiResponse<SubjectGroupResponse>.NotFound($"Không tìm thấy tổ - bộ môn có id {id}");
            }
            var teacher = _context.Users.Where(u => u.Role.Name.ToLower() == "teacher").FirstOrDefault(x => x.Id == request.TeacherId);
            if (teacher == null)
            {
                return ApiResponse<SubjectGroupResponse>.NotFound($"Teacher có id {request.TeacherId} không tồn tại");
            }

            if (request?.subjectId == null || request.subjectId.Any(id => id.GetType() != typeof(int)))
            {
                return ApiResponse<SubjectGroupResponse>.Fail("Vui lòng truyền vào mảng là số nguyên!!!");
            }
            _mapper.Map(request, subjectGroup);
           
            List<Subject> listSubjects = new List<Subject>();
            foreach (var idSubject in request.subjectId)
            {
                var subject = _context.Subjects.FirstOrDefault(x => x.Id == idSubject);
                if (subject == null)
                {
                    return ApiResponse<SubjectGroupResponse>.NotFound($"Môn học có id {idSubject} không tồn tại");
                }
                else
                {
                    var checkSubject = _context.SubjectSubjectGroups.FirstOrDefault(ssg => ssg.SubjectId == subject.Id && ssg.SubjectGroupId == subjectGroup.Id);
                    if(checkSubject == null)
                    {
                        listSubjects.Add(subject);
                    }
                }
            }
            var update = _subjectGroupRepo.UpdateSubjectGroup(subjectGroup);

            List<SubjectSubjectGroup> subjectSubjectGroups = new List<SubjectSubjectGroup>();

            foreach (var subject in listSubjects)
            {
                subjectSubjectGroups.Add(new SubjectSubjectGroup
                {
                    SubjectId = subject.Id,
                    SubjectGroupId = update.Id
                });
            }

            _context.AddRange(subjectSubjectGroups);
            _context.SaveChanges();

            return ApiResponse<SubjectGroupResponse>.Success(_mapper.Map<SubjectGroupResponse>(update));
        }

        public ApiResponse<string> DeleteSubjectGroup(long id)
        {
            var delete = _subjectGroupRepo.DeleteSubjectGroup(id);
            if (delete)
            {
                return new ApiResponse<string>(0, "Xóa tổ - bộ môn thành công", null, null);
            }
            else
            {
                return ApiResponse<string>.NotFound($"Không tìm thấy tổ - bộ môn có id {id}");
            }
        }

        public ApiResponse<string> DeleteSubject(long? subjectGroupId, long? subjectId)
        {
            if (subjectGroupId == null)
            {
                return ApiResponse<string>.NotFound("Vui lòng truyền subjectGroupId!!!");
            }
            if (subjectId == null)
            {
                return ApiResponse<string>.NotFound("Vui lòng truyền subjectId!!!");
            }
            var subjectGroup = _subjectGroupRepo.GetSubjectGroupById(subjectGroupId.Value);
            if (subjectGroup == null) {
                return ApiResponse<string>.NotFound($"Tổ bộ môn có id {subjectGroupId.Value} không tồn tại!!!");
            }
            var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId.Value);
            if (subject == null)
            {
                return ApiResponse<string>.NotFound($"Môn học có id {subjectId.Value} không tồn tại!!!");
            }

            var subjectSubjectGroup = _context.SubjectSubjectGroups.Where(ssg => ssg.SubjectGroupId == subjectGroup.Id && ssg.SubjectId == subject.Id);
            if (!subjectSubjectGroup.ToList().Any())
            {
                return ApiResponse<string>.NotFound($"Môn học trong tổ bộ môn không tồn tại theo id subjectGroupId là {subjectGroup.Id} và subjectId là {subject.Id}!!!");
            }

            _context.SubjectSubjectGroups.RemoveRange(subjectSubjectGroup);
            _context.SaveChanges();

            return new ApiResponse<string>(0, "Xóa bộ môn khỏi tổ bộ môn thành công", null, null);
        }
    }
}
