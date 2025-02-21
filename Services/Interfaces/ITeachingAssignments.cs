using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITeachingAssignmentsService
    {
        ApiResponse<ICollection<TeachingAssignmentsResponse>> GetTeachingAssignments(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TeachingAssignmentsResponse> GetTeachingAssignmentById(long id);
        ApiResponse<TeachingAssignmentsResponse> CreateTeachingAssignment(TeachingAssignmentsRequest teachingAssignmentRequest);
        ApiResponse<TeachingAssignmentsResponse> UpdateTeachingAssignment(long id, TeachingAssignmentsRequest teachingAssignmentRequest);
        ApiResponse<bool> DeleteTeachingAssignment(long id);
    }
}
