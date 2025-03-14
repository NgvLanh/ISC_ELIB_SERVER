using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITeachingAssignmentsService
    {
        ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignments(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<TeachingAssignmentsResponse> GetTeachingAssignmentById(int id);
        ApiResponse<TeachingAssignmentsResponse> CreateTeachingAssignment(TeachingAssignmentsRequest teachingAssignmentRequest);
        ApiResponse<TeachingAssignmentsResponse> UpdateTeachingAssignment(int id, TeachingAssignmentsRequest teachingAssignmentRequest);
        ApiResponse<bool> DeleteTeachingAssignment(int id);
    }
}
