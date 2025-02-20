using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

public class ExamScheduleMapper : Profile
{
    public ExamScheduleMapper()
    {
        CreateMap<ExamScheduleRequest, ExamSchedule>();
        CreateMap<ExamSchedule, ExamScheduleResponse>();
    }
}
