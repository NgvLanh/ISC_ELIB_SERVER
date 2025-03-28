using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassTypeService
    {
        ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ClassTypeResponse> GetClassTypeById(long id);
        ApiResponse<ClassTypeResponse> GetClassTypeByName(string name);
        ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest);
        ApiResponse<ClassTypeResponse> UpdateClassType(long id, ClassTypeRequest classTypeRequest);
        ApiResponse<bool> DeleteClassType(long id);
    }

    public class ClassTypeService : IClassTypeService
    {
        private readonly IClassTypeRepo _repository;
        private readonly IMapper _mapper;

        public ClassTypeService(IClassTypeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //
        public ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetClassTypes().AsQueryable();

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

            var response = _mapper.Map<ICollection<ClassTypeResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ClassTypeResponse>>.Success(response)
                : ApiResponse<ICollection<ClassTypeResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ClassTypeResponse> GetClassTypeById(long id)
        {
            var classType = _repository.GetClassTypeById(id);
            if (classType == null)
            {
                return ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
            }
            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType));
        }

        public ApiResponse<ClassTypeResponse> GetClassTypeByName(string name)
        {
            var classType = _repository.GetClassTypes().FirstOrDefault(ct => ct.Name == name);
            if (classType == null)
            {
                return ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp với tên này");
            }
            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType));
        }

        public ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest)
        {
            var classType = _mapper.Map<ClassType>(classTypeRequest);
            _repository.CreateClassType(classType);
            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType));
        }

        public ApiResponse<ClassTypeResponse> UpdateClassType(long id, ClassTypeRequest classTypeRequest)
        {
            var existingClassType = _repository.GetClassTypeById(id);
            if (existingClassType == null)
            {
                return ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
            }

            existingClassType.Name = classTypeRequest.Name;
            existingClassType.Description = classTypeRequest.Description;

            var updatedClassType = _repository.UpdateClassType(existingClassType);
            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(updatedClassType));
        }

        public ApiResponse<bool> DeleteClassType(long id)
        {
            var deleted = _repository.DeleteClassType(id);
            if (!deleted)
            {
                return ApiResponse<bool>.NotFound("Không tìm thấy loại lớp để xóa");
            }
            return ApiResponse<bool>.Success(true);
        }
    }
}
