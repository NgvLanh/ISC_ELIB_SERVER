using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassesService
    {
        ApiResponse<ICollection<ClassesResponse>> GetClass(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ClassesResponse> GetClassById(long id);
        ApiResponse<ClassesResponse> GetClassByName(string name);
        ApiResponse<ClassesResponse> CreateClass(ClassesRequest classesRequests);
        ApiResponse<ClassesResponse> UpdateClass(long id, ClassesRequest classesRequests);
        ApiResponse<bool> DeleteClass(long id);

    }
    public class ClassesService : IClassesService
    {
        private readonly IClassesRepo _repository;
        private readonly IMapper _mapper;



        public ClassesService(IClassesRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ClassesResponse> CreateClass(ClassesRequest classesRequests)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<bool> DeleteClass(long id)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<ICollection<ClassesResponse>> GetClass(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetClass().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<ClassesResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ClassesResponse>>.Success(response)
                : ApiResponse<ICollection<ClassesResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ClassesResponse> GetClassById(long id)
        {
            var seach = _repository.GetClassById(id);
            return seach != null
                ? ApiResponse<ClassesResponse>.Success(_mapper.Map<ClassesResponse>(seach))
                : ApiResponse<ClassesResponse>.NotFound($"Không tìm thấy");
        }

        public ApiResponse<ClassesResponse> GetClassByName(string name)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<ClassesResponse> UpdateClass(long id, ClassesRequest classesRequests)
        {
            throw new NotImplementedException();
        }
    }
}
