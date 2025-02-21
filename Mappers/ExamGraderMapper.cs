using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

public class ExamGraderMapper : Profile
{
    public ExamGraderMapper()
    {
        CreateMap<ExamGrader, ExamGraderResponse>();
        CreateMap<ExamGraderRequest, ExamGrader>();
    }
}
