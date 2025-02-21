using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class ClassesService : IClassesService
    {
        private readonly IClassesRepo _repository;
        private readonly IMapper _mapper;

        public ClassesService(IClassesRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ClassesResponse>> GetClass(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetClass().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(c => c.Id) : query.OrderBy(c => c.Id),
                _ => query.OrderBy(c => c.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<ClassesResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ClassesResponse>>.Success(response)
                : ApiResponse<ICollection<ClassesResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ClassesResponse> GetClassById(long id)
        {
            var classData = _repository.GetClassById(id);
            return classData != null
                ? ApiResponse<ClassesResponse>.Success(_mapper.Map<ClassesResponse>(classData))
                : ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học");
        }

        public ApiResponse<ClassesResponse> GetClassByName(string name)
        {
            var classData = _repository.GetClass().FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            return classData != null
                ? ApiResponse<ClassesResponse>.Success(_mapper.Map<ClassesResponse>(classData))
                : ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học với tên này");
        }

        public ApiResponse<ClassesResponse> CreateClass(ClassesRequest classesRequest)
        {
            var existingClass = _repository.GetClass().FirstOrDefault(c => c.Name.ToLower() == classesRequest.Name.ToLower());
            if (existingClass != null)
            {
                return ApiResponse<ClassesResponse>.Conflict("Tên lớp học đã tồn tại");
            }

            var newClass = new Class
            {
                Name = classesRequest.Name,
                Description = classesRequest.Description,
            };

            var createdClass = _repository.CreateClass(newClass);
            return ApiResponse<ClassesResponse>.Success(_mapper.Map<ClassesResponse>(createdClass));
        }

        public ApiResponse<ClassesResponse> UpdateClass(long id, ClassesRequest classesRequest)
        {
            var existingClass = _repository.GetClassById(id);
            if (existingClass == null)
            {
                return ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học");
            }

            var duplicate = _repository.GetClass().FirstOrDefault(c => c.Name.ToLower() == classesRequest.Name.ToLower() && c.Id != id);
            if (duplicate != null)
            {
                return ApiResponse<ClassesResponse>.Conflict("Tên lớp học đã tồn tại");
            }

            existingClass.Name = classesRequest.Name;
            existingClass.Description = classesRequest.Description;

            var updatedClass = _repository.UpdateClass(existingClass);
            return ApiResponse<ClassesResponse>.Success(_mapper.Map<ClassesResponse>(updatedClass));
        }

        public ApiResponse<bool> DeleteClass(long id)
        {
            var deleted = _repository.DeleteClass(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy lớp học để xóa");
        }
    }
}
