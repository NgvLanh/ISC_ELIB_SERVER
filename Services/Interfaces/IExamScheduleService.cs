using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IExamScheduleService
    {
        ApiResponse<ICollection<ExamScheduleResponse>> GetExamSchedules(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ExamScheduleResponse> GetExamScheduleById(int id);
        ApiResponse<ICollection<ExamScheduleResponse>> GetExamScheduleByName(string name);
        ApiResponse<ExamScheduleResponse> CreateExamSchedule(ExamScheduleRequest request);
        ApiResponse<ExamScheduleResponse> UpdateExamSchedule(int id, ExamScheduleRequest request);
        ApiResponse<ExamScheduleResponse> DeleteExamSchedule(int id);
    }
}
