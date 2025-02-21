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

        public ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignments(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTeachingAssignments().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Description.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "StartDate" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.StartDate) : query.OrderBy(t => t.StartDate),
                "EndDate" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.EndDate) : query.OrderBy(t => t.EndDate),
                _ => query.OrderBy(t => t.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<TeachingAssignmentsResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TeachingAssignmentsResponse>>.Success(response)
                : ApiResponse<ICollection<TeachingAssignmentsResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<TeachingAssignmentsResponse> GetTeachingAssignmentById(long id)
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

        public ApiResponse<TeachingAssignmentsResponse> UpdateTeachingAssignment(long id, TeachingAssignmentsRequest request)
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
            existingAssignment.Active = request.Active;

            var updatedAssignment = _repository.UpdateTeachingAssignment(existingAssignment);
            return ApiResponse<TeachingAssignmentsResponse>.Success(_mapper.Map<TeachingAssignmentsResponse>(updatedAssignment));
        }

        public ApiResponse<bool> DeleteTeachingAssignment(long id)
        {
            var deleted = _repository.DeleteTeachingAssignment(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy dữ liệu để xóa");
        }
    }
}
