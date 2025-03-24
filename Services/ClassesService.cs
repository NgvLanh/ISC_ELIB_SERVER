using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Services
{
    public class ClassesService : IClassesService
    {
        private readonly IClassesRepo _repository;
        private readonly IClassSubjectRepo _classSubjectRepo;
        private readonly IMapper _mapper;

        public ClassesService(IClassesRepo repository, IClassSubjectRepo classSubjectRepo, IMapper mapper)
        {
            _repository = repository;
            _classSubjectRepo = classSubjectRepo;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ClassesResponse>> GetClass(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetClass()
                .Include(c => c.GradeLevel)
                .Include(c => c.AcademicYear)
                .Include(c => c.User)
                .Include(c => c.ClassType)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .Include(c => c.Users)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.Users)
                    .ThenInclude(u => u.UserStatus) 
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

            var classDict = response.ToDictionary(c => c.Id);

            foreach (var classData in result)
            {
                if (classDict.TryGetValue(classData.Id, out var classResponse))
                {
                    classResponse.Subjects = classData.ClassSubjects
                        .Select(cs => new ClassSubjectResponse
                        {
                            Id = cs.Id,
                            Code = cs.Subject.Code,
                            Name = cs.Subject.Name,
                            HoursSemester1 = (int)cs.Subject.HoursSemester1,
                            HoursSemester2 = (int)cs.Subject.HoursSemester2
                        })
                        .ToList();

                    classResponse.Student = classData.Users
                        .Where(u => u.RoleId == 3)
                        .Select(u => new ClassUserResponse
                        {
                            Id = u.Id,
                            Code = u.Code,
                            FullName = u.FullName,
                            EnrollmentDate = u.EnrollmentDate.HasValue
                                ? u.EnrollmentDate.Value.ToString("dd/MM/yyyy")
                                : null,
                            Year = (u.AcademicYear?.StartTime.HasValue == true && u.AcademicYear?.EndTime.HasValue == true)
                                ? $"{u.AcademicYear.StartTime.Value.Year}-{u.AcademicYear.EndTime.Value.Year}"
                                : null,
                            UserStatus = u.UserStatus.Name
                        })
                        .ToList();
                }
            }

            return result.Any()
                ? ApiResponse<ICollection<ClassesResponse>>.Success(response, page, pageSize, totalCount)
                : ApiResponse<ICollection<ClassesResponse>>.NotFound("Không có dữ liệu");
        }



        public ApiResponse<ClassesResponse> GetClassById(int id)
        {
            var classData = _repository.GetClass()
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .Include(c => c.Users) 
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.Users)
                    .ThenInclude(u => u.UserStatus)
                .FirstOrDefault(c => c.Id == id);

            if (classData == null)
            {
                return ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học");
            }

            var response = _mapper.Map<ClassesResponse>(classData);

            response.Subjects = classData.ClassSubjects
                .Select(cs => new ClassSubjectResponse
                {
                    Id = cs.Id,
                    Code = cs.Subject.Code,
                    Name = cs.Subject.Name,
                    HoursSemester1 = (int)cs.Subject.HoursSemester1,
                    HoursSemester2 = (int)cs.Subject.HoursSemester2
                })
                .ToList();

            response.Student = classData.Users
                .Where(u => u.RoleId == 3)
                .Select(u => new ClassUserResponse
                {
                    Id = u.Id,
                    Code = u.Code,
                    FullName = u.FullName,
                    EnrollmentDate = u.EnrollmentDate.HasValue
                        ? u.EnrollmentDate.Value.ToString("dd/MM/yyyy")
                        : null,
                    Year = (u.AcademicYear?.StartTime.HasValue == true && u.AcademicYear?.EndTime.HasValue == true)
                        ? $"{u.AcademicYear.StartTime.Value.Year}-{u.AcademicYear.EndTime.Value.Year}"
                        : null,

                    UserStatus = u.UserStatus.Name
                    
                })
                .ToList();

            return ApiResponse<ClassesResponse>.Success(response);
        }



        public ApiResponse<ClassesResponse> GetClassByName(string name)
        {
            var classData = _repository.GetClass().FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            return classData != null
                ? ApiResponse<ClassesResponse>.Success(_mapper.Map<ClassesResponse>(classData))
                : ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học với tên này");
        }

        public async Task<ApiResponse<ClassesResponse>> CreateClassAsync(ClassesRequest classesRequest)
        {
            if (classesRequest == null)
            {
                return ApiResponse<ClassesResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }

            bool isNameExist = await _repository.GetClass()
                .AnyAsync(c => c.Name.ToLower() == classesRequest.Name.ToLower());

            if (isNameExist)
            {
                return ApiResponse<ClassesResponse>.Conflict("Tên lớp học đã tồn tại");
            }

            var classEntity = _mapper.Map<Class>(classesRequest);

            try
            {
                await _repository.CreateClassAsync(classEntity);
                await _repository.SaveChangesAsync();

                if (classEntity.Id == 0)
                {
                    return ApiResponse<ClassesResponse>.BadRequest("Không thể tạo lớp học, ID không hợp lệ");
                }

                await UpdateClassSubjectsAsync(classEntity.Id, classesRequest.Subjects);

                classEntity = await _repository.GetClass()
                    .Include(c => c.ClassSubjects)
                        .ThenInclude(cs => cs.Subject)
                    .Include(c => c.Users)
                        .ThenInclude(u => u.UserStatus)
                    .FirstOrDefaultAsync(c => c.Id == classEntity.Id);

                if (classEntity == null)
                {
                    return ApiResponse<ClassesResponse>.BadRequest("Lỗi khi tải lại lớp học sau khi tạo");
                }

                var response = _mapper.Map<ClassesResponse>(classEntity);

                response.Subjects = classEntity.ClassSubjects?.Select(cs => new ClassSubjectResponse
                {
                    Id = cs.Id,
                    Code = cs.Subject.Code,
                    Name = cs.Subject.Name,
                    HoursSemester1 = cs.Subject.HoursSemester1 ?? 0,
                    HoursSemester2 = cs.Subject.HoursSemester2 ?? 0
                }).ToList() ?? new List<ClassSubjectResponse>();

               
                response.Student = classEntity.Users?
                    .Where(u => u.RoleId == 3)
                    .Select(u => new ClassUserResponse
                    {
                        Id = u.Id,
                        Code = u.Code,
                        FullName = u.FullName,
                        EnrollmentDate = u.EnrollmentDate.HasValue
                            ? u.EnrollmentDate.Value.ToString("dd/MM/yyyy")
                            : null,
                        Year = (u.AcademicYear?.StartTime.HasValue == true && u.AcademicYear?.EndTime.HasValue == true)
                            ? $"{u.AcademicYear.StartTime.Value.Year}-{u.AcademicYear.EndTime.Value.Year}"
                            : null,
                        UserStatus = u.UserStatus.Name
                    }).ToList() ?? new List<ClassUserResponse>();

                return ApiResponse<ClassesResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassesResponse>.BadRequest($"Lỗi khi tạo lớp học: {ex.Message}");
            }
        }





        public async Task<ApiResponse<ClassesResponse>> UpdateClassAsync(int id, ClassesRequest classesRequest)
        {
            if (classesRequest == null)
            {
                return ApiResponse<ClassesResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }

            bool isNameExist = await _repository.GetClass()
                .AnyAsync(c => c.Name.ToLower() == classesRequest.Name.ToLower() && c.Id != id);

            if (isNameExist)
            {
                return ApiResponse<ClassesResponse>.Conflict("Tên lớp học đã tồn tại");
            }

            var classEntity = await _repository.GetClass()
                .Include(c => c.Users)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
            {
                return ApiResponse<ClassesResponse>.NotFound("Không tìm thấy lớp học");
            }

            _mapper.Map(classesRequest, classEntity);
            classEntity.Active = true;

            try
            {
                await _repository.UpdateClassAsync(classEntity);
                await _repository.SaveChangesAsync();

                await UpdateClassSubjectsAsync(classEntity.Id, classesRequest.Subjects);

                classEntity = await _repository.GetClass()
                    .Include(c => c.ClassSubjects)
                        .ThenInclude(cs => cs.Subject)
                    .Include(c => c.Users)
                        .ThenInclude(u => u.UserStatus)
                    .FirstOrDefaultAsync(c => c.Id == classEntity.Id);

                if (classEntity == null)
                {
                    return ApiResponse<ClassesResponse>.BadRequest("Lỗi khi tải lại lớp học sau khi cập nhật");
                }

                var response = _mapper.Map<ClassesResponse>(classEntity);

                response.Subjects = classEntity.ClassSubjects?
                    .Select(cs => new ClassSubjectResponse
                    {
                        Id = cs.Id,
                        Code = cs.Subject.Code,
                        Name = cs.Subject.Name,
                        HoursSemester1 = cs.Subject.HoursSemester1 ?? 0,
                        HoursSemester2 = cs.Subject.HoursSemester2 ?? 0
                    }).ToList() ?? new List<ClassSubjectResponse>();

                response.Student = classEntity.Users?
                 .Where(u => u.RoleId == 3)
                 .Select(u => new ClassUserResponse
                 {
                     Id = u.Id,
                     Code = u.Code,
                     FullName = u.FullName,
                     EnrollmentDate = u.EnrollmentDate.HasValue
                         ? u.EnrollmentDate.Value.ToString("dd/MM/yyyy")
                         : null,
                     Year = u.AcademicYear != null && u.AcademicYear.StartTime.HasValue && u.AcademicYear.EndTime.HasValue
                         ? $"{u.AcademicYear.StartTime.Value.Year}-{u.AcademicYear.EndTime.Value.Year}"
                         : "Không có thông tin", // ✅ Đảm bảo không bị null
                     UserStatus = u.UserStatus != null ? u.UserStatus.Name : "Không có trạng thái"
                 }).ToList() ?? new List<ClassUserResponse>();


                return ApiResponse<ClassesResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClassesResponse>.BadRequest($"Lỗi khi cập nhật lớp học: {ex.Message}");
            }
        }



        public async Task<ApiResponse<bool>> UpdateClassSubjectsAsync(int classId, List<int> subjectIds)
        {
            try
            {
                var classEntity = await _repository.GetClass()
                    .Include(c => c.ClassSubjects)
                    .FirstOrDefaultAsync(c => c.Id == classId);

                if (classEntity == null)
                {
                    return ApiResponse<bool>.NotFound("Không tìm thấy lớp học");
                }

                await _classSubjectRepo.RemoveClassSubjectsByClassIdAsync(classId);

                if (subjectIds != null && subjectIds.Any())
                {
                    await _classSubjectRepo.AddClassSubjectsAsync(classId, subjectIds);
                }

                classEntity = await _repository.GetClass()
                    .Include(c => c.ClassSubjects)
                        .ThenInclude(cs => cs.Subject)
                    .FirstOrDefaultAsync(c => c.Id == classId);

                return ApiResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.BadRequest($"Lỗi khi cập nhật môn học: {ex.Message}");
            }
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
