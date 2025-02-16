using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class ClassTypeService : IClassTypeService
    {
        private readonly IClassTypeRepo _repository;
        private readonly IMapper _mapper;

        public ClassTypeService(IClassTypeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetClassTypes().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(ct => ct.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ct => ct.Name) : query.OrderBy(ct => ct.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ct => ct.Id) : query.OrderBy(ct => ct.Id),
                _ => query.OrderBy(ct => ct.Id)
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
            return classType != null
                ? ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType))
                : ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp");
        }

        public ApiResponse<ClassTypeResponse> GetClassTypeByName(string name)
        {
            var classType = _repository.GetClassTypes().FirstOrDefault(ct => ct.Name.ToLower() == name.ToLower());
            return classType != null
                ? ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(classType))
                : ApiResponse<ClassTypeResponse>.NotFound("Không tìm thấy loại lớp với tên này");
        }

        public ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest)
        {
            var existingClassType = _repository.GetClassTypes().FirstOrDefault(ct => ct.Name.ToLower() == classTypeRequest.Name.ToLower());
            if (existingClassType != null)
            {
                return ApiResponse<ClassTypeResponse>.Conflict("Tên loại lớp đã tồn tại");
            }

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

            var duplicate = _repository.GetClassTypes().FirstOrDefault(ct => ct.Name.ToLower() == classTypeRequest.Name.ToLower() && ct.Id != id);
            if (duplicate != null)
            {
                return ApiResponse<ClassTypeResponse>.Conflict("Tên loại lớp đã tồn tại");
            }

            existingClassType.Name = classTypeRequest.Name;
            existingClassType.Description = classTypeRequest.Description;

            var updatedClassType = _repository.UpdateClassType(existingClassType);
            return ApiResponse<ClassTypeResponse>.Success(_mapper.Map<ClassTypeResponse>(updatedClassType));
        }

        public ApiResponse<bool> DeleteClassType(long id)
        {
            var deleted = _repository.DeleteClassType(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy loại lớp để xóa");
        }
    }
}
