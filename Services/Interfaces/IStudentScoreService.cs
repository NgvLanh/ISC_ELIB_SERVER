using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using System.Collections.Generic;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IStudentScoreService
    {
        ApiResponse<ICollection<StudentScoreResponse>> GetStudentScores(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<StudentScoreResponse> GetStudentScoreById(int id);
        ApiResponse<StudentScoreResponse> CreateStudentScore(StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScoreResponse> UpdateStudentScore(int id, StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScore> DeleteStudentScore(int id);
        ApiResponse<StudentScoreDashboardResponse> ViewStudentDashboardScores(int? academicYearId, int? classId, int? gradeLevelId, int? subjectId);
    }
}
