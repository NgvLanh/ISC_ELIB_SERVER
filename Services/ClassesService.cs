using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using Microsoft.EntityFrameworkCore;
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

            var response = result.Select(c => new ClassesResponse
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                StudentQuantity = c.StudentQuantity,
                SubjectQuantity = c.SubjectQuantity,
                Description = c.Description,
                Active = c.Active,
                GradeLevel = c.GradeLevel != null ? new GradeLevelResponse
                {
                    Id = c.GradeLevel.Id,
                    Name = c.GradeLevel.Name,
                    Code = c.GradeLevel.Code,
                    TeacherId = c.GradeLevel.Teacher?.Id.ToString()
                } : null,

                AcademicYear = c.AcademicYear != null ? new AcademicYearResponse
                {
                    Id = c.AcademicYear.Id,
                    StartTime = (DateTime)(c.AcademicYear.StartTime != DateTime.MinValue ? c.AcademicYear.StartTime : null),
                    EndTime = (DateTime)(c.AcademicYear.EndTime != DateTime.MinValue ? c.AcademicYear.EndTime : null),
                    SchoolId = (int)(c.AcademicYear.School?.Id)
                } : null,

                User = c.User != null ? new UserResponse
                {
                    Id = c.User.Id,
                    Code = c.User.Code,
                    FullName = c.User.FullName ?? "",
                    Gender = c.User.Gender,
                    Email = string.IsNullOrEmpty(c.User.Email) ? null : c.User.Email,
                    PhoneNumber = string.IsNullOrEmpty(c.User.PhoneNumber) ? null : c.User.PhoneNumber,
                    PlaceBirth = string.IsNullOrEmpty(c.User.PlaceBirth) ? null : c.User.PlaceBirth,
                    Nation = string.IsNullOrEmpty(c.User.Nation) ? null : c.User.Nation,
                    Religion = string.IsNullOrEmpty(c.User.Religion) ? null : c.User.Religion,
                    AddressFull = string.IsNullOrEmpty(c.User.AddressFull) ? null : c.User.AddressFull,
                    Street = string.IsNullOrEmpty(c.User.Street) ? null : c.User.Street,
                    Dob = c.User.Dob != DateTime.MinValue ? c.User.Dob : null,
                    EnrollmentDate = c.User.EnrollmentDate != DateTime.MinValue ? c.User.EnrollmentDate : null,
                    RoleId = (int)(c.User.Role?.Id),
                    AcademicYearId = (int)(c.User.AcademicYear?.Id),
                    ClassId = (int)(c.User.Class?.Id),
                } : null,

                ClassType = c.ClassType != null ? new ClassTypeResponse
                {
                    Id = c.ClassType.Id,
                    Name = c.ClassType.Name,
                    Description = c.ClassType.Description,
                    Status = c.ClassType.Status
                } : null

            }).ToList();

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

            var response = new ClassesResponse
            {
                Id = classData.Id,
                Code = classData.Code,
                Name = classData.Name,
                Active = classData.Active,
                StudentQuantity = classData.StudentQuantity,
                SubjectQuantity = classData.SubjectQuantity,
                Description = classData.Description,

                GradeLevel = classData.GradeLevel != null ? new GradeLevelResponse
                {
                    Id = classData.GradeLevel.Id,
                    Name = classData.GradeLevel.Name,
                    Code = classData.GradeLevel.Code,
                    TeacherId = classData.GradeLevel.Teacher?.Id.ToString()
                } : null,

                AcademicYear = classData.AcademicYear != null ? new AcademicYearResponse
                {
                    Id = classData.AcademicYear.Id,
                    StartTime = (DateTime)(classData.AcademicYear.StartTime != DateTime.MinValue ? classData.AcademicYear.StartTime : null),
                    EndTime = (DateTime)(classData.AcademicYear.EndTime != DateTime.MinValue ? classData.AcademicYear.EndTime : null),
                    SchoolId = (int)(classData.AcademicYear.School?.Id)
                } : null,

                User = classData.User != null ? new UserResponse
                {
                    Id = classData.User.Id,
                    Code = classData.User.Code,
                    FullName = classData.User.FullName ?? "",
                    Gender = classData.User.Gender,
                    Email = string.IsNullOrEmpty(classData.User.Email) ? null : classData.User.Email,
                    PhoneNumber = string.IsNullOrEmpty(classData.User.PhoneNumber) ? null : classData.User.PhoneNumber,
                    PlaceBirth = string.IsNullOrEmpty(classData.User.PlaceBirth) ? null : classData.User.PlaceBirth,
                    Nation = string.IsNullOrEmpty(classData.User.Nation) ? null : classData.User.Nation,
                    Religion = string.IsNullOrEmpty(classData.User.Religion) ? null : classData.User.Religion,
                    AddressFull = string.IsNullOrEmpty(classData.User.AddressFull) ? null : classData.User.AddressFull,
                    Street = string.IsNullOrEmpty(classData.User.Street) ? null : classData.User.Street,
                    Dob = classData.User.Dob != DateTime.MinValue ? classData.User.Dob : null,
                    EnrollmentDate = classData.User.EnrollmentDate != DateTime.MinValue ? classData.User.EnrollmentDate : null,
                    RoleId = (int)(classData.User.Role?.Id),
                    AcademicYearId = (int)(classData.User.AcademicYear?.Id),
                    ClassId = (int)(classData.User.Class?.Id),


                } : null,

                ClassType = classData.ClassType != null ? new ClassTypeResponse
                {
                    Id = classData.ClassType.Id,
                    Name = classData.ClassType.Name,
                    Description = classData.ClassType.Description,
                    Status = classData.ClassType.Status
                } : null
            };

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
