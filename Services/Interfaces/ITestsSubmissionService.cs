using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITestsSubmissionService
    {
        ApiResponse<ICollection<TestsSubmissionResponse>> GetTestsSubmissiones(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TestsSubmissionResponse> GetTestsSubmissionById(long id);
        //ApiResponse<TestsSubmissionResponse> GetTestsSubmissionByName(string name);
        //ApiResponse<TestsSubmissionResponse> GetTestsSubmissionByTestId(long testId);
        Task<ApiResponse<List<TestsSubmissionResponse>>> GetByTestIdAsync(int testId);
        ApiResponse<TestsSubmissionResponse> CreateTestsSubmission(TestsSubmissionRequest TestsSubmissionRequest);
        ApiResponse<TestsSubmissionResponse> UpdateTestsSubmission(long id, TestsSubmissionRequest TestsSubmission);
        ApiResponse<TestsSubmission> DeleteTestsSubmission(long id);
    }
}
