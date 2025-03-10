using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class TeacherListService : ITeacherListService
    {
        private readonly TeacherListRepo _repository;
        private readonly IMapper _mapper;

        public TeacherListService(TeacherListRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public ApiResponse<TeacherListResponse> GetTeacherListById(int id)
        {
            var teacherInfo = _repository.GetTeacherListById(id);
            return teacherInfo != null
                ? ApiResponse<TeacherListResponse>.Success(_mapper.Map<TeacherListResponse>(teacherInfo))
                : ApiResponse<TeacherListResponse>.NotFound($"Không tìm thấy giáo viên với ID #{id}");
        }

        public ApiResponse<ICollection<TeacherListResponse>> GetTeacherLists(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAllTeacherList();

            // Tìm kiếm: theo tên (User.FullName) hoặc mã giáo viên (User.Code)
            if (!string.IsNullOrEmpty(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(t =>
                    t.User != null && (
                        (t.User.FullName != null && t.User.FullName.ToLower().Contains(lowerSearch)) ||
                        (t.User.Code != null && t.User.Code.ToLower().Contains(lowerSearch))
                    )
                );
            }

            // Sắp xếp
            switch (sortColumn)
            {
                case "TeacherCode":
                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(t => t.User.Code)
                        : query.OrderBy(t => t.User.Code);
                    break;
                case "FullName":
                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(t => t.User.FullName)
                        : query.OrderBy(t => t.User.FullName);
                    break;
                default:
                    query = sortOrder.ToLower() == "desc"
                        ? query.OrderByDescending(t => t.Id)
                        : query.OrderBy(t => t.Id);
                    break;
            }

            // Phân trang
            var result = query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            var response = _mapper.Map<ICollection<TeacherListResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TeacherListResponse>>.Success(response)
                : ApiResponse<ICollection<TeacherListResponse>>.NotFound("Không có dữ liệu");
        }
    }
}
