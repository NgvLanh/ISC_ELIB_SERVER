using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using System.Collections.Generic;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IStudentScoreService
    {
        ApiResponse<ICollection<StudentScoreResponse>> GetStudentScores(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<StudentScoreResponse> GetStudentScoreById(long id);
        ApiResponse<StudentScoreResponse> CreateStudentScore(StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScoreResponse> UpdateStudentScore(long id, StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScore> DeleteStudentScore(long id);
    }
}
