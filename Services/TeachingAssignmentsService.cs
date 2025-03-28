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
        private readonly IMapper _mapper;

        public TeachingAssignmentsService(ITeachingAssignmentsRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsClassStatusTrue(
            int? page, int? pageSize, string? sortColumn, string? sortOrder,
            string? searchSubject, int? subjectId, int? subjectGroupId)
        {
            try
            {
                var query = _repository.GetTeachingAssignments()
                    .Include(ta => ta.User)
                    .Include(ta => ta.Class)
                    .Include(ta => ta.Subject)
                    .Include(ta => ta.Subject.SubjectGroup)
                    .Include(ta => ta.Topics)
                    .Include(ta => ta.Sessions)
                    .Where(ta => ta.Class != null && ta.Class.Active)
                    .AsQueryable()
                    .AsNoTracking();

                if (subjectId.HasValue)
                {
                    query = query.Where(us => us.SubjectId == subjectId.Value);
                }

                if (subjectGroupId.HasValue)
                {
                    query = query.Where(us => us.Subject.SubjectGroupId == subjectGroupId.Value);
                }

                if (!string.IsNullOrWhiteSpace(searchSubject))
                {
                    query = query.Where(us => us.Subject.Name.Contains(searchSubject));
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

                        assignmentResponse.SubjectGroup = new SubjectGroupResponse
                        {
                            Id = assignment.Subject.SubjectGroup.Id,
                            Name = assignment.Subject.SubjectGroup.Name
                        };

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

                    }
                }

                return result.Any()
                    ? ApiResponse<ICollection<TeachingAssignmentsResponse>>.Success(response, page, pageSize, totalRecords)
                    : ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Không có dữ liệu");
            }
            catch (Exception ex)
            {
                return ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Lỗi:" +ex.Message);

            }
        }

        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignmentsClassStatusFalse(
            int? page, int? pageSize, string? sortColumn, string? sortOrder,
            string? searchSubject, int? subjectId, int? subjectGroupId)
        {
            try
            {
                var query = _repository.GetTeachingAssignments()
                    .Include(ta => ta.User)
                    .Include(ta => ta.Class)
                    .Include(ta => ta.Subject)
                    .Include(ta => ta.Subject.SubjectGroup)
                    .Include(ta => ta.Topics)
                    .Include(ta => ta.Sessions)
                    .Where(ta => ta.Class != null && !ta.Class.Active)
                    .AsQueryable()
                    .AsNoTracking();

                if (subjectId.HasValue)
                {
                    query = query.Where(us => us.SubjectId == subjectId.Value);
                }

                if (subjectGroupId.HasValue)
                {
                    query = query.Where(us => us.Subject.SubjectGroupId == subjectGroupId.Value);
                }

                if (!string.IsNullOrWhiteSpace(searchSubject))
                {
                    query = query.Where(us => us.Subject.Name.Contains(searchSubject));
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

                        assignmentResponse.SubjectGroup = new SubjectGroupResponse
                        {
                            Id = assignment.Subject.SubjectGroup.Id,
                            Name = assignment.Subject.SubjectGroup.Name
                        };

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
            var assignment = _repository.GetTeachingAssignmentById(id);
            return assignment != null
                ? ApiResponse<TeachingAssignmentsResponse>.Success(_mapper.Map<TeachingAssignmentsResponse>(assignment))
                : ApiResponse<TeachingAssignmentsResponse>.NotFound("Không tìm thấy dữ liệu");
        }

        public ApiResponse<TeachingAssignmentsResponse> CreateTeachingAssignment(TeachingAssignmentsRequest request)
        {
            var newAssignment = _mapper.Map<TeachingAssignment>(request);
            var createdAssignment = _repository.CreateTeachingAssignment(newAssignment);
            return ApiResponse<TeachingAssignmentsResponse>.Success(_mapper.Map<TeachingAssignmentsResponse>(createdAssignment));
        }

        public ApiResponse<TeachingAssignmentsResponse> UpdateTeachingAssignment(int id, TeachingAssignmentsRequest request)
        {
            var existingAssignment = _repository.GetTeachingAssignmentById(id);
            if (existingAssignment == null)
            {
                return ApiResponse<TeachingAssignmentsResponse>.NotFound("Không tìm thấy dữ liệu");
            }

            existingAssignment.StartDate = request.StartDate;
            existingAssignment.EndDate = request.EndDate;
            existingAssignment.Description = request.Description;
            existingAssignment.UserId = request.UserId;
            existingAssignment.ClassId = request.ClassId;
            existingAssignment.SubjectId = request.SubjectId;
            existingAssignment.TopicsId = request.TopicsId;
            existingAssignment.SemesterId = request.SemesterId;

            var updatedAssignment = _repository.UpdateTeachingAssignment(existingAssignment);
            return ApiResponse<TeachingAssignmentsResponse>.Success(_mapper.Map<TeachingAssignmentsResponse>(updatedAssignment));
        }

        public ApiResponse<bool> DeleteTeachingAssignment(int id)
        {
            var deleted = _repository.DeleteTeachingAssignment(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy dữ liệu để xóa");
        }
    }
}
