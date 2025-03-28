using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models.Responses;
using ISC_ELIB_SERVER.Requests;
using ISC_ELIB_SERVER.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IExamScheduleClassService
    {
        ApiResponse<ICollection<ExamScheduleClassResponse>> GetExamScheduleClasses(
            int page,
            int pageSize,
            int? classId,
            int? exampleSchedule,
            int? supervisoryTeacherId,
            string sortColumn,
            string sortOrder);

        ApiResponse<ExamScheduleClassResponse> GetExamScheduleClassById(int id);

        ApiResponse<ICollection<ExamScheduleClassResponse>> GetExamScheduleClassByFilter(
            int? classId,
            int? exampleSchedule,
            int? supervisoryTeacherId);

        ApiResponse<ExamScheduleClassResponse> CreateExamScheduleClass(ExamScheduleClassRequest request);

        ApiResponse<ExamScheduleClassResponse> UpdateExamScheduleClass(int id, ExamScheduleClassRequest request);

        ApiResponse<ExamScheduleClassResponse> DeleteExamScheduleClass(int id);
    }
}
