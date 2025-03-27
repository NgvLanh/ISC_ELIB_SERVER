﻿    using AutoMapper;
    using ISC_ELIB_SERVER.DTOs.Requests;
    using ISC_ELIB_SERVER.DTOs.Responses;
    using ISC_ELIB_SERVER.Models;
    using ISC_ELIB_SERVER.Repositories;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml;

    namespace ISC_ELIB_SERVER.Services
    {
        public class ClassesService : IClassesService
        {
            private readonly IClassesRepo _repository;
            private readonly IClassSubjectRepo _classSubjectRepo;
            private readonly AcademicYearRepo _academicYearRepo;
            private readonly GradeLevelRepo _gradeLevelRepo;
            private readonly UserRepo _userRepo;
            private readonly IClassTypeRepo _classTypeRepo;
            private readonly IMapper _mapper;

            public ClassesService(IClassesRepo repository, IClassSubjectRepo classSubjectRepo, IMapper mapper, AcademicYearRepo academicYearRepo, GradeLevelRepo gradeLevelRepo,UserRepo userRepo, IClassTypeRepo classTypeRepo)
            {
                _repository = repository;
                _classSubjectRepo = classSubjectRepo;
                _mapper = mapper;
                _academicYearRepo = academicYearRepo;
                _gradeLevelRepo = gradeLevelRepo;
                _userRepo = userRepo;
                _classTypeRepo = classTypeRepo;
            }

            public ApiResponse<ICollection<ClassesResponse>> GetClass(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
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

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(c => c.Name.Contains(search));
                }

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
                             : "Không có thông tin",
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
                        var subjects = await _repository.GetSubjects()
                            .Where(s => subjectIds.Contains(s.Id))
                            .ToListAsync();

                        var classSubjects = subjects.Select(s => new ClassSubject
                        {
                            ClassId = classId,
                            SubjectId = s.Id,
                            HoursSemester1 = s.HoursSemester1 ?? 0,
                            HoursSemester2 = s.HoursSemester2 ?? 0
                        }).ToList();

                        await _classSubjectRepo.AddClassSubjectsAsync(classSubjects);
                    }

                    await _repository.SaveChangesAsync();

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



        public ApiResponse<bool> DeleteClass(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return ApiResponse<bool>.BadRequest("Danh sách ID lớp học không được để trống");
            }

            try
            {
                bool deleted = _repository.DeleteClasses(ids);
                return deleted
                    ? ApiResponse<bool>.Success(true)
                    : ApiResponse<bool>.NotFound("Không tìm thấy lớp học để xóa");
            }
            catch (DbUpdateException ex)
            {
                return ApiResponse<bool>.BadRequest("Không thể xóa lớp học do ràng buộc dữ liệu");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error("Lỗi máy chủ: " + ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> ImportClassesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return ApiResponse<bool>.BadRequest("File không hợp lệ");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;

                        var newClasses = new List<Class>();

                        for (int row = 2; row <= rowCount; row++) 
                        {
                            var code = worksheet.Cells[row, 1].Text.Trim();
                            var name = worksheet.Cells[row, 2].Text.Trim();
                            var studentQuantity = worksheet.Cells[row, 3].GetValue<int?>();
                            var subjectQuantity = worksheet.Cells[row, 4].GetValue<int?>();
                            var description = worksheet.Cells[row, 5].Text.Trim();
                            var gradeLevelId = worksheet.Cells[row, 6].GetValue<int?>();
                            var academicYearId = worksheet.Cells[row, 7].GetValue<int?>();
                            var userId = worksheet.Cells[row, 8].GetValue<int?>();
                            var classTypeId = worksheet.Cells[row, 9].GetValue<int?>();

                            if (string.IsNullOrWhiteSpace(name))
                            {
                                continue;
                            }

                            var gradeLevel = _gradeLevelRepo.GetGradeLevels()
                                .FirstOrDefault(g => g.Id == gradeLevelId);

                            var academicYear = _academicYearRepo.GetAcademicYears()
                                .FirstOrDefault(ay => ay.Id== academicYearId);

                            var user = _userRepo.GetUsers()
                                .FirstOrDefault(u => u.Id == userId);

                            var classType = _classTypeRepo.GetClassTypes()
                                .FirstOrDefault(ct => ct.Id == classTypeId);

                            bool isExist = false;
                            if (gradeLevel != null && academicYear != null)
                            {
                                isExist = await _repository.GetClass()
                                    .AnyAsync(c => c.Name == name
                                                && c.GradeLevelId == gradeLevel.Id
                                                && c.AcademicYearId == academicYear.Id);
                            }

                            if (isExist)
                            {
                                continue;
                            }

                            var newClass = new Class
                            {
                                Code = code,
                                Name = name,
                                StudentQuantity = studentQuantity,
                                SubjectQuantity = subjectQuantity,
                                Description = description,
                                GradeLevelId = gradeLevel?.Id,
                                AcademicYearId = academicYear?.Id,
                                UserId = user?.Id,
                                ClassTypeId = classType?.Id,
                                Active = true
                            };

                            newClasses.Add(newClass);
                        }

                        if (newClasses.Any())
                        {
                            await _repository.AddRangeAsync(newClasses);
                            await _repository.SaveChangesAsync();
                        }
                    }
                }

                return ApiResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.BadRequest($"Lỗi khi import file: {ex.Message}");
            }
        }
    }
}
