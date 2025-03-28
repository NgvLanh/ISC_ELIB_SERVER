using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IExamGraderService
    {
        ApiResponse<ICollection<ExamGraderResponse>> GetExamGraders(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ExamGraderResponse> GetExamGraderById(int id);
        ApiResponse<ICollection<ExamGraderResponse>> GetExamGraderByExamId(int examId);
        ApiResponse<ExamGraderResponse> CreateExamGrader(ExamGraderRequest request);
        ApiResponse<ExamGraderResponse> UpdateExamGrader(int id, ExamGraderRequest request);
        ApiResponse<ExamGraderResponse> DeleteExamGrader(int id);
    }
}
