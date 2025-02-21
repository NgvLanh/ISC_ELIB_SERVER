using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Service
{
    public interface IExamGraderService
    {
        Task<List<ExamGraderResponse>> GetAllAsync(int page, int pageSize, string? sortBy, bool isDescending, int? examId, int? userId);
        Task<ExamGraderResponse?> GetByIdAsync(int id);
        Task AddAsync(ExamGraderRequest request);
        Task UpdateAsync(int id, ExamGraderRequest request);
        Task DeleteAsync(int id);
    }
}
