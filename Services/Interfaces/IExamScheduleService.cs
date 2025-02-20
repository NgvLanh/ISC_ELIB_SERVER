using System.Collections.Generic;
using System.Threading.Tasks;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface IExamScheduleService
    {
        IEnumerable<ExamScheduleResponse> GetExamSchedules();
        ExamScheduleResponse? GetExamScheduleById(int id);
        void CreateExamSchedule(ExamScheduleRequest request);
        void UpdateExamSchedule(int id, ExamScheduleRequest request);
        void DeleteExamSchedule(int id);
    }
}
