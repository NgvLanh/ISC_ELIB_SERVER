using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Services
{
    public class TeachingAssignmentsService : ITeachingAssignmentsService
    {
        private readonly ITeachingAssignmentsRepo _repository;
        private readonly isc_dbContext _context;
        private readonly IMapper _mapper;

        public TeachingAssignmentsService(ITeachingAssignmentsRepo repository, IMapper mapper, isc_dbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsNotExpired(
         int? page, int? pageSize, string? sortColumn, string? sortOrder,
         string? searchSubject, int? subjectId, int? subjectGroupId)
        {
            //try
            {
                var query = _repository.GetTeachingAssignments()
                    .Include(ta => ta.User)
                    .Include(ta => ta.Class)
                    .Include(ta => ta.Subject)
                        .ThenInclude(s => s.SubjectSubjectGroups)
                            .ThenInclude(ssg => ssg.SubjectGroup)
                    .Include(ta => ta.Topics)
                    .Include(ta => ta.Sessions)
                    .Include(ta => ta.Semester)
                    .ThenInclude(s => s.AcademicYear)
                    .Where(ta => ta.EndDate >= DateTime.Now) // Chỉ lấy chưa qua EndDate
                    .AsQueryable()
                    .AsNoTracking();

                if (subjectId.HasValue)
                {
                    query = query.Where(us => us.SubjectId == subjectId.Value);
                }

                if (subjectGroupId.HasValue)
                {
                    query = query.Where(us => us.Subject.SubjectSubjectGroups.Any(ssg => ssg.SubjectGroup.Id == subjectGroupId.Value));
                }

                if (!string.IsNullOrWhiteSpace(searchSubject))
                {
                    query = query.Where(us => us.Subject.Name.ToLower().Contains(searchSubject.ToLower()));

                }

                int totalRecords = query.Count();

                sortColumn = string.IsNullOrEmpty(sortColumn) ? "Id" : sortColumn;
                sortOrder = sortOrder?.ToLower() ?? "asc";

                query = sortColumn switch
                {
                    "Id" => sortOrder == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                    "SubjectName" => sortOrder == "desc" ? query.OrderByDescending(us => us.Subject.Name) : query.OrderBy(us => us.Subject.Name),
                    _ => query.OrderBy(us => us.Id)
                };

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var result = query.ToList();
                var response = _mapper.Map<ICollection<TeachingAssignmentsResponse>>(result);

                var assignmentsDict = response.ToDictionary(ta => ta.Id);

                foreach (var assignment in result)
                {
                    if (assignmentsDict.TryGetValue(assignment.Id, out var assignmentResponse))
                    {
                        assignmentResponse.User = new TeachingAssignmentsResponse.TeachingAssignmentsUserResponse
                        {
                            Id = assignment.User.Id,
                            Code = assignment.User.Code,
                            FullName = assignment.User.FullName
                        };

                        assignmentResponse.Class = new TeachingAssignmentsResponse.TeachingAssignmentsClassResponse
                        {
                            Id = assignment.Class.Id,
                            Code = assignment.Class.Code,
                            Name = assignment.Class.Name
                        };

                        assignmentResponse.Subject = new TeachingAssignmentsResponse.TeachingAssignmentsSubjectResponse
                        {
                            Id = assignment.Subject.Id,
                            Name = assignment.Subject.Name
                        };

                        assignmentResponse.SubjectGroup = assignment.Subject.SubjectSubjectGroups
                            .Select(s => new SubjectGroupResponse
                            {
                                Id = s.SubjectGroup.Id,
                                Name = s.SubjectGroup.Name
                            }).ToList();

                        assignmentResponse.Topics = new TeachingAssignmentsResponse.TeachingAssignmentsTopicResponse
                        {
                            Id = assignment.Topics.Id,
                            Name = assignment.Topics.Name
                        };

                        assignmentResponse.Sessions = assignment.Sessions
                            .Select(s => new TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).ToList();

                        if (assignment.Semester != null)
                        {
                            assignmentResponse.Semester = new TeachingAssignmentsResponse.TeachingAssignmentsSemesterResponse
                            {
                                Id = assignment.Semester.Id,
                                Name = assignment.Semester.Name,
                                AcademicYear = assignment.Semester.AcademicYear != null
                                    ? new AcademicYearResponse
                                    {
                                        Id = assignment.Semester.AcademicYear.Id,
                                        StartTime = (DateTime)assignment.Semester.AcademicYear.StartTime,
                                        EndTime = (DateTime)assignment.Semester.AcademicYear.EndTime,
                                    }
                                    : null
                            };
                        }
                    }
                }

                return result.Any()
                    ? ApiResponse<ICollection<TeachingAssignmentsResponse>>.Success(response, page, pageSize, totalRecords)
                    : ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Không có dữ liệu");
            }
            //catch (Exception ex)
            //{
            //    return ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Lỗi: " + ex.Message);
            //}
        }


        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsExpired(
            int? page, int? pageSize, string? sortColumn, string? sortOrder,
            string? searchSubject, int? subjectId, int? subjectGroupId)
        {
            try
            {
                var query = _repository.GetTeachingAssignments()
                    .Include(ta => ta.User)
                    .Include(ta => ta.Class)
                    .Include(ta => ta.Subject)
                        .ThenInclude(s => s.SubjectSubjectGroups)
                            .ThenInclude(ssg => ssg.SubjectGroup)
                    .Include(ta => ta.Topics)
                    .Include(ta => ta.Sessions)
                    .Include(ta => ta.Semester)
                    .ThenInclude(s => s.AcademicYear)
                    .Where(ta => ta.EndDate < DateTime.Now)
                    .AsQueryable()
                    .AsNoTracking();

                if (subjectId.HasValue)
                {
                    query = query.Where(us => us.SubjectId == subjectId.Value);
                }

                if (subjectGroupId.HasValue)
                {
                    //var subjectGroup = _context.SubjectSubjectGroups.FirstOrDefault(s => s.SubjectGroupId == subjectGroupId.Value);
                    query = query.Where(us => us.Subject.SubjectSubjectGroups.Any(ssg => ssg.SubjectGroup.Id == subjectGroupId.Value));
                }

                if (!string.IsNullOrWhiteSpace(searchSubject))
                {
                    query = query.Where(us => us.Subject.Name.ToLower().Contains(searchSubject.ToLower()));
                }

                int totalRecords = query.Count();

                sortColumn = string.IsNullOrEmpty(sortColumn) ? "Id" : sortColumn;
                sortOrder = sortOrder?.ToLower() ?? "asc";

                query = sortColumn switch
                {
                    "Id" => sortOrder == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                    "SubjectName" => sortOrder == "desc" ? query.OrderByDescending(us => us.Subject.Name) : query.OrderBy(us => us.Subject.Name),
                    _ => query.OrderBy(us => us.Id)
                };

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var result = query.ToList();
                var response = _mapper.Map<ICollection<TeachingAssignmentsResponse>>(result);

                var assignmentsDict = response.ToDictionary(ta => ta.Id);

                foreach (var assignment in result)
                {
                    if (assignmentsDict.TryGetValue(assignment.Id, out var assignmentResponse))
                    {
                        assignmentResponse.User = new TeachingAssignmentsResponse.TeachingAssignmentsUserResponse
                        {
                            Id = assignment.User.Id,
                            Code = assignment.User.Code,
                            FullName = assignment.User.FullName
                        };

                        assignmentResponse.Class = new TeachingAssignmentsResponse.TeachingAssignmentsClassResponse
                        {
                            Id = assignment.Class.Id,
                            Code = assignment.Class.Code,
                            Name = assignment.Class.Name
                        };

                        assignmentResponse.Subject = new TeachingAssignmentsResponse.TeachingAssignmentsSubjectResponse
                        {
                            Id = assignment.Subject.Id,
                            Name = assignment.Subject.Name
                        };

                        assignmentResponse.SubjectGroup = assignment.Subject.SubjectSubjectGroups
                            .Select(s => new SubjectGroupResponse
                            {
                                Id = s.SubjectGroup.Id,
                                Name = s.SubjectGroup.Name

                            }).ToList();

                        assignmentResponse.Topics = new TeachingAssignmentsResponse.TeachingAssignmentsTopicResponse
                        {
                            Id = assignment.Topics.Id,
                            Name = assignment.Topics.Name
                        };

                        assignmentResponse.Sessions = assignment.Sessions
                            .Select(s => new TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).ToList();

                        if (assignment.Semester != null)
                        {
                            assignmentResponse.Semester = new TeachingAssignmentsResponse.TeachingAssignmentsSemesterResponse
                            {
                                Id = assignment.Semester.Id,
                                Name = assignment.Semester.Name,
                                AcademicYear = assignment.Semester.AcademicYear != null
                                    ? new AcademicYearResponse
                                    {
                                        Id = assignment.Semester.AcademicYear.Id,
                                        StartTime = (DateTime)assignment.Semester.AcademicYear.StartTime,
                                        EndTime = (DateTime)assignment.Semester.AcademicYear.EndTime,
                                    }
                                    : null
                            };
                        }
                    }
                }

                return result.Any()
                    ? ApiResponse<ICollection<TeachingAssignmentsResponse>>.Success(response, page, pageSize, totalRecords)
                    : ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Không có dữ liệu");
            }
            catch (Exception ex)
            {
                return ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Lỗi:" + ex.Message);

            }
        }

        public ApiResponse<TeachingAssignmentsResponse> GetTeachingAssignmentById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ApiResponse<TeachingAssignmentsResponse>.BadRequest("ID không hợp lệ");
                }

                var assignment = _repository.GetTeachingAssignments()
                    .Include(ta => ta.User)
                    .Include(ta => ta.Class)
                    .Include(ta => ta.Subject)
                        .ThenInclude(s => s.SubjectSubjectGroups)
                            .ThenInclude(ssg => ssg.SubjectGroup)
                    .Include(ta => ta.Topics)
                    .Include(ta => ta.Sessions)
                    .Include(ta => ta.Semester)
                    .ThenInclude(s => s.AcademicYear)
                    .AsNoTracking()
                    .FirstOrDefault(ta => ta.Id == id);

                return assignment != null
                    ? ApiResponse<TeachingAssignmentsResponse>.Success(_mapper.Map<TeachingAssignmentsResponse>(assignment))
                    : ApiResponse<TeachingAssignmentsResponse>.NotFound("Không tìm thấy dữ liệu");
            }
            catch (Exception ex)
            {
                return ApiResponse<TeachingAssignmentsResponse>.NotFound("Lỗi hệ thống: " + ex.Message);
            }
        }


        public ApiResponse<TeachingAssignmentsResponse> CreateTeachingAssignment(TeachingAssignmentsRequest request)
        {
            try
            {
                var newAssignment = _mapper.Map<TeachingAssignment>(request);
                var createdAssignment = _repository.CreateTeachingAssignment(newAssignment);

                if (createdAssignment == null)
                {
                    return ApiResponse<TeachingAssignmentsResponse>.NotFound("Không tìm thấy phân công giảng dạy.");
                }

                var response = _mapper.Map<TeachingAssignmentsResponse>(createdAssignment);

                response.User = new TeachingAssignmentsResponse.TeachingAssignmentsUserResponse
                {
                    Id = createdAssignment.User.Id,
                    Code = createdAssignment.User.Code,
                    FullName = createdAssignment.User.FullName
                };

                response.Class = new TeachingAssignmentsResponse.TeachingAssignmentsClassResponse
                {
                    Id = createdAssignment.Class.Id,
                    Code = createdAssignment.Class.Code,
                    Name = createdAssignment.Class.Name
                };

                response.Subject = new TeachingAssignmentsResponse.TeachingAssignmentsSubjectResponse
                {
                    Id = createdAssignment.Subject.Id,
                    Name = createdAssignment.Subject.Name
                };

                response.SubjectGroup = createdAssignment.Subject.SubjectSubjectGroups
                    .Select(s => new SubjectGroupResponse
                    {
                        Id = s.SubjectGroup.Id,
                        Name = s.SubjectGroup.Name,
                        TeacherId = (int)s.SubjectGroup.TeacherId
                    }).ToList();

                response.Topics = new TeachingAssignmentsResponse.TeachingAssignmentsTopicResponse
                {
                    Id = createdAssignment.Topics.Id,
                    Name = createdAssignment.Topics.Name
                };

                response.Sessions = createdAssignment.Sessions
                    .Select(s => new TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList();

                if (createdAssignment.Semester != null)
                {
                    response.Semester = new TeachingAssignmentsResponse.TeachingAssignmentsSemesterResponse
                    {
                        Id = createdAssignment.Semester.Id,
                        Name = createdAssignment.Semester.Name,
                        AcademicYear = createdAssignment.Semester.AcademicYear != null
                            ? new AcademicYearResponse
                            {
                                Id = createdAssignment.Semester.AcademicYear.Id,
                                StartTime = (DateTime)createdAssignment.Semester.AcademicYear.StartTime,
                                EndTime = (DateTime)createdAssignment.Semester.AcademicYear.EndTime,
                            }
                            : null
                    };
                }

                return ApiResponse<TeachingAssignmentsResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TeachingAssignmentsResponse>.NotFound("Lỗi hệ thống: " + ex.Message);
            }
        }

        public ApiResponse<TeachingAssignmentsResponse> UpdateTeachingAssignment(int id, TeachingAssignmentsRequest request)
        {
            try
            {
                var existingAssignment = _repository.GetTeachingAssignmentById(id);
                if (existingAssignment == null)
                {
                    return ApiResponse<TeachingAssignmentsResponse>.NotFound("Không tìm thấy dữ liệu");
                }

                _mapper.Map(request, existingAssignment);
                existingAssignment.Active = true;
                var updatedAssignment = _repository.UpdateTeachingAssignment(existingAssignment);

                updatedAssignment = _repository.GetTeachingAssignmentById(updatedAssignment.Id);

                if (updatedAssignment == null)
                {
                    return ApiResponse<TeachingAssignmentsResponse>.NotFound("Không tìm thấy phân công giảng dạy.");
                }

                var response = _mapper.Map<TeachingAssignmentsResponse>(updatedAssignment);

                response.User = updatedAssignment.User != null
                    ? new TeachingAssignmentsResponse.TeachingAssignmentsUserResponse
                    {
                        Id = updatedAssignment.User.Id,
                        Code = updatedAssignment.User.Code,
                        FullName = updatedAssignment.User.FullName
                    }
                    : null;

                response.Class = updatedAssignment.Class != null
                    ? new TeachingAssignmentsResponse.TeachingAssignmentsClassResponse
                    {
                        Id = updatedAssignment.Class.Id,
                        Code = updatedAssignment.Class.Code,
                        Name = updatedAssignment.Class.Name
                    }
                    : null;

                response.Subject = updatedAssignment.Subject != null
                    ? new TeachingAssignmentsResponse.TeachingAssignmentsSubjectResponse
                    {
                        Id = updatedAssignment.Subject.Id,
                        Name = updatedAssignment.Subject.Name
                    }
                    : null;

                response.SubjectGroup = updatedAssignment.Subject.SubjectSubjectGroups
                   .Select(s => new SubjectGroupResponse
                   {
                       Id = s.SubjectGroup.Id,
                       Name = s.SubjectGroup.Name,
                       TeacherId = (int)s.SubjectGroup.TeacherId
                   }).ToList();

                response.Topics = updatedAssignment.Topics != null
                    ? new TeachingAssignmentsResponse.TeachingAssignmentsTopicResponse
                    {
                        Id = updatedAssignment.Topics.Id,
                        Name = updatedAssignment.Topics.Name
                    }
                    : null;

                response.Sessions = updatedAssignment.Sessions != null
                    ? updatedAssignment.Sessions.Select(s => new TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList()
                    : new List<TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse>();

                response.Semester = updatedAssignment.Semester != null
                    ? new TeachingAssignmentsResponse.TeachingAssignmentsSemesterResponse
                    {
                        Id = updatedAssignment.Semester.Id,
                        Name = updatedAssignment.Semester.Name,
                        AcademicYear = updatedAssignment.Semester.AcademicYear != null
                            ? new AcademicYearResponse
                            {
                                Id = updatedAssignment.Semester.AcademicYear.Id,
                                StartTime = (DateTime)updatedAssignment.Semester.AcademicYear.StartTime,
                                EndTime = (DateTime)updatedAssignment.Semester.AcademicYear.EndTime
                            }
                            : null
                    }
                    : null;

                return ApiResponse<TeachingAssignmentsResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TeachingAssignmentsResponse>.NotFound("Lỗi hệ thống: " + ex.Message);
            }
        }


        public ApiResponse<bool> DeleteTeachingAssignment(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return ApiResponse<bool>.BadRequest("Danh sách ID phân công giảng dạy không được để trống");
            }

            try
            {
                bool deleted = _repository.DeleteTeachingAssignment(ids);
                return deleted
                    ? ApiResponse<bool>.Success(true)
                    : ApiResponse<bool>.NotFound("Không tìm thấy dữ liệu để xóa");
            }
            catch (DbUpdateException ex)
            {
                return ApiResponse<bool>.BadRequest("Không thể xóa phân công giảng dạy do ràng buộc dữ liệu");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Error("Lỗi máy chủ: " + ex.Message);
            }
        }


        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeacherByAcademicYearAndSubjectGroup(
            int? academicYearId, int? subjectGroupId,
            int? page, int? pageSize, string? sortColumn, string? sortOrder, string? search)
        {
            try
            {
                var subjectSubjectGroup = _context.SubjectSubjectGroups.Where(s => s.SubjectGroup.Id == subjectGroupId);
                var query = _repository.GetTeachingAssignments()
                    .Include(ta => ta.User)
                    .Include(ta => ta.Class)
                    .Include(ta => ta.Subject)
                        .ThenInclude(s => s.SubjectSubjectGroups)
                            .ThenInclude(ssg => ssg.SubjectGroup)
                    .Include(ta => ta.Topics)
                    .Include(ta => ta.Sessions)
                    .Include(ta => ta.Semester)
                    .ThenInclude(s => s.AcademicYear)
                    .Where(ta => ta.Semester.AcademicYear.Id == academicYearId &&
                                 ta.Subject.SubjectSubjectGroups.Any(ssg => ssg.SubjectGroupId == subjectGroupId))
                    .AsQueryable()
                    .AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string lowerSearch = search.ToLower();
                    query = query.Where(ta => ta.User.FullName.ToLower().Contains(lowerSearch));
                }

                int totalRecords = query.Count();

                sortColumn = string.IsNullOrEmpty(sortColumn) ? "Id" : sortColumn;
                sortOrder = sortOrder?.ToLower() ?? "asc";

                query = sortColumn switch
                {
                    "Id" => sortOrder == "desc" ? query.OrderByDescending(ta => ta.Id) : query.OrderBy(ta => ta.Id),
                    "TeacherName" => sortOrder == "desc" ? query.OrderByDescending(ta => ta.User.FullName) : query.OrderBy(ta => ta.User.FullName),
                    _ => query.OrderBy(ta => ta.Id)
                };

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var result = query.ToList();
                var response = _mapper.Map<ICollection<TeachingAssignmentsResponse>>(result);

                foreach (var assignment in response)
                {
                    var originalAssignment = result.FirstOrDefault(a => a.Id == assignment.Id);
                    if (originalAssignment != null)
                    {
                        assignment.User = new TeachingAssignmentsResponse.TeachingAssignmentsUserResponse
                        {
                            Id = originalAssignment.User.Id,
                            Code = originalAssignment.User.Code,
                            FullName = originalAssignment.User.FullName
                        };

                        assignment.Class = new TeachingAssignmentsResponse.TeachingAssignmentsClassResponse
                        {
                            Id = originalAssignment.Class.Id,
                            Code = originalAssignment.Class.Code,
                            Name = originalAssignment.Class.Name
                        };

                        assignment.Subject = new TeachingAssignmentsResponse.TeachingAssignmentsSubjectResponse
                        {
                            Id = originalAssignment.Subject.Id,
                            Name = originalAssignment.Subject.Name
                        };

                        assignment.SubjectGroup = originalAssignment.Subject.SubjectSubjectGroups
                            .Select(s => new SubjectGroupResponse
                            {
                                Id = s.SubjectGroup.Id,
                                Name = s.SubjectGroup.Name
                            }).ToList();
                  
                        assignment.Topics = new TeachingAssignmentsResponse.TeachingAssignmentsTopicResponse
                        {
                            Id = originalAssignment.Topics.Id,
                            Name = originalAssignment.Topics.Name
                        };

                        assignment.Sessions = originalAssignment.Sessions
                            .Select(s => new TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse
                            {
                                Id = s.Id,
                                Name = s.Name
                            }).ToList();

                        if (originalAssignment.Semester != null)
                        {
                            assignment.Semester = new TeachingAssignmentsResponse.TeachingAssignmentsSemesterResponse
                            {
                                Id = originalAssignment.Semester.Id,
                                Name = originalAssignment.Semester.Name,
                                AcademicYear = originalAssignment.Semester.AcademicYear != null
                                    ? new AcademicYearResponse
                                    {
                                        Id = originalAssignment.Semester.AcademicYear.Id,
                                        StartTime = (DateTime)originalAssignment.Semester.AcademicYear.StartTime,
                                        EndTime = (DateTime)originalAssignment.Semester.AcademicYear.EndTime,
                                    }
                                    : null
                            };
                        }
                    }
                }

                return response.Any()
                    ? ApiResponse<ICollection<TeachingAssignmentsResponse>>.Success(response, page, pageSize, totalRecords)
                    : ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Không có dữ liệu");
            }
            catch (Exception ex)
            {
                return ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Lỗi: " + ex.Message);
            }
        }

        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsByTeacher(
            int? page, int? pageSize, string? sortColumn, string? sortOrder, int? teacherId)
        {
            try
            {
                if (!teacherId.HasValue)
                {
                    return ApiResponse<ICollection<TeachingAssignmentsResponse>>.BadRequest("Giảng viên ID là bắt buộc.");
                }

                var query = _repository.GetTeachingAssignments()
                    .Include(ta => ta.User)
                    .Include(ta => ta.Class)
                    .Include(ta => ta.Subject)
                        .ThenInclude(s => s.SubjectSubjectGroups)
                            .ThenInclude(ssg => ssg.SubjectGroup)
                    .Include(ta => ta.Semester)
                    .ThenInclude(s => s.AcademicYear)
                    .Where(ta => ta.User.Id == teacherId.Value)
                    .AsQueryable()
                    .AsNoTracking();

                int totalRecords = query.Count();

                sortColumn = string.IsNullOrEmpty(sortColumn) ? "Id" : sortColumn;
                sortOrder = sortOrder?.ToLower() ?? "asc";

                query = sortColumn switch
                {
                    "Id" => sortOrder == "desc" ? query.OrderByDescending(ta => ta.Id) : query.OrderBy(ta => ta.Id),
                    "ClassName" => sortOrder == "desc" ? query.OrderByDescending(ta => ta.Class.Name) : query.OrderBy(ta => ta.Class.Name),
                    "SubjectName" => sortOrder == "desc" ? query.OrderByDescending(ta => ta.Subject.Name) : query.OrderBy(ta => ta.Subject.Name),
                    _ => query.OrderBy(ta => ta.Id)
                };

                if (page.HasValue && pageSize.HasValue && page > 0 && pageSize > 0)
                {
                    query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                }

                var result = query.ToList();
                var response = _mapper.Map<ICollection<TeachingAssignmentsResponse>>(result);

                return result.Any()
                    ? ApiResponse<ICollection<TeachingAssignmentsResponse>>.Success(response, page, pageSize, totalRecords)
                    : ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Không có dữ liệu");
            }
            catch (Exception ex)
            {
                return ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Lỗi: " + ex.Message);
            }
        }

    }
}
