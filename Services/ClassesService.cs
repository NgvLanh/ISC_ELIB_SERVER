using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            var query = _repository.GetClass()
                .Include(c => c.GradeLevel)
                .Include(c => c.AcademicYear)
                .Include(c => c.User)
                .Include(c => c.ClassType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                bool isDesc = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

                query = sortColumn.ToLower() switch
                {
                    "code" => isDesc ? query.OrderByDescending(c => c.Code) : query.OrderBy(c => c.Code),
                    "name" => isDesc ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                    "studentquantity" => isDesc ? query.OrderByDescending(c => c.StudentQuantity) : query.OrderBy(c => c.StudentQuantity),
                    _ => query.OrderBy(c => c.Code)
                };
            }

            int totalCount = query.Count();

            if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<ClassesResponse>>(result);
            return result.Any()
                ? ApiResponse<ICollection<ClassesResponse>>.Success(response, page, pageSize, totalCount)
                : ApiResponse<ICollection<ClassesResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<ClassesResponse> GetClassById(int id)
        {
            var classData = _repository.GetClassById(id);
            if (classData == null)
            {
                return ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học");
            }
            var subjects = classData.ClassSubjects.Select(cs => cs.Subject).ToList();
            var response = _mapper.Map<ClassesResponse>(classData);
            response.Subjects = _mapper.Map<ICollection<ClassSubjectResponse>>(subjects);

            return ApiResponse<ClassesResponse>.Success(response);
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
            var existingClass = _repository.GetClass()
                .FirstOrDefault(c => c.Name.ToLower() == classesRequest.Name.ToLower());

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
            var existingClass = _repository.GetClass()
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType)
                .FirstOrDefault(c => c.Id == id);

            if (existingClass == null)
            {
                return ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học");
            }

            var duplicate = _repository.GetClass()
                .FirstOrDefault(c => c.Name.ToLower() == classesRequest.Name.ToLower() && c.Id != id);

            if (duplicate != null)
            {
                return ApiResponse<ClassesResponse>.Conflict("Tên lớp học đã tồn tại");
            }

            existingClass.Name = classesRequest.Name;
            existingClass.Description = classesRequest.Description;
            existingClass.StudentQuantity = classesRequest.StudentQuantity;
            existingClass.SubjectQuantity = classesRequest.SubjectQuantity;
            existingClass.GradeLevelId = classesRequest.GradeLevelId;
            existingClass.AcademicYearId = classesRequest.AcademicYearId;

            var updatedClass = _repository.UpdateClass(existingClass);

            if (updatedClass == null)
            {
                return ApiResponse<ClassesResponse>.BadRequest("Lỗi khi cập nhật lớp học");
            }

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
