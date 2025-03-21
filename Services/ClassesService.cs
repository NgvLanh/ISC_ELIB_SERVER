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

        public ApiResponse<ICollection<ClassesResponse>> GetClass(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetClass().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(ay => ay.Id)
            };


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<ClassesResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<ClassesResponse>>
            .Success(response, page, pageSize, _repository.GetClass().Count)
             : ApiResponse<ICollection<ClassesResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ClassesResponse> GetClassById(int id)
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
                Code = classesRequest.Code,
                StudentQuantity = classesRequest.StudentQuantity,
                SubjectQuantity = classesRequest.SubjectQuantity,
                GradeLevelId = classesRequest.GradeLevelId,
                AcademicYearId = classesRequest.AcademicYearId,
                UserId = classesRequest?.UserId,
                ClassTypeId = classesRequest?.ClassTypeId,
            };
            try
            {
                var createdClass = _repository.CreateClass(newClass);
                return ApiResponse<ClassesResponse>.Success(_mapper.Map<ClassesResponse>(createdClass));
            }
            catch
            {
                return ApiResponse<ClassesResponse>.BadRequest("Kiểm tra lại các khóa ngoại");
            }

        }

        public ApiResponse<ClassesResponse> UpdateClass(int id, ClassesRequest classesRequest)
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

        public ApiResponse<bool> DeleteClass(int id)
        {
            var deleted = _repository.DeleteClass(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy lớp học để xóa");
        }
    }
}
