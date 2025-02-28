using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITestSubmissionAnswerService
    {
        ApiResponse<ICollection<TestSubmissionAnswerResponse>> GetTestSubmissionAnswers(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TestSubmissionAnswerResponse> GetTestSubmissionAnswerById(int id);
        ApiResponse<TestSubmissionAnswerResponse> CreateTestSubmissionAnswer(TestSubmissionAnswerRequest testSubmissionAnswerRequest);
        ApiResponse<TestSubmissionAnswerResponse> UpdateTestSubmissionAnswer(int id, TestSubmissionAnswerRequest testSubmissionAnswerRequest);
        ApiResponse<TestSubmissionAnswerResponse> DeleteTestSubmissionAnswer(int id);
    }
}
