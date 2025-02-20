using ISC_ELIB_SERVER.Dto.Request;
using ISC_ELIB_SERVER.Dto.Response;
using ISC_ELIB_SERVER.Requests;
using ISC_ELIB_SERVER.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Service
{
    public interface IExamScheduleClassService
    {
        Task<IEnumerable<ExamScheduleClassResponse>> GetAllAsync();
        Task<ExamScheduleClassResponse?> GetByIdAsync(int id);
        Task<ExamScheduleClassResponse> AddAsync(ExamScheduleClassRequest request);
        Task<ExamScheduleClassResponse?> UpdateAsync(int id, ExamScheduleClassRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
