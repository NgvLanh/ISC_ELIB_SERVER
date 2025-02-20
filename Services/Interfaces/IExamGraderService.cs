using System.Collections.Generic;
using System.Threading.Tasks;
using ISC_ELIB_SERVER.DTOs.Responses;

public interface IExamGraderService
{
    Task<List<ExamGraderResponse>> GetAllExamGradersAsync();
    Task<ExamGraderResponse?> GetExamGraderByIdAsync(int id);
    Task<ExamGraderResponse> CreateExamGraderAsync(ExamGraderRequest request);
    Task<ExamGraderResponse?> UpdateExamGraderAsync(int id, ExamGraderRequest request);
    Task<bool> DeleteExamGraderAsync(int id);
}
