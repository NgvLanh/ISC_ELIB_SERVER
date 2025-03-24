using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

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

        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignments(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetTeachingAssignments().AsQueryable();

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

            var response = _mapper.Map<ICollection<TeachingAssignmentsResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<TeachingAssignmentsResponse>>
            .Success(response, page, pageSize, _repository.GetTeachingAssignments().Count)
             : ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Không có dữ liệu");
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
