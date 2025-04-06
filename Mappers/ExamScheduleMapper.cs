using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ExamScheduleMapper : Profile
    {
        public ExamScheduleMapper()
        {
            CreateMap<ExamSchedule, ExamScheduleResponse>()
                .ForMember(dest => dest.AcademicYear,
                    opt => opt.MapFrom(src => src.AcademicYear != null
                        ? $"{src.AcademicYear.StartTime:yyyy} - {src.AcademicYear.EndTime:yyyy}"
                        : null))
                .ForMember(dest => dest.SubjectName,
                    opt => opt.MapFrom(src => src.SubjectNavigation != null ? src.SubjectNavigation.Name : null))
                .ForMember(dest => dest.Semester,
                    opt => opt.MapFrom(src => src.Semester != null ? src.Semester.Name : null))
                .ForMember(dest => dest.GradeLevel,
                    opt => opt.MapFrom(src => src.GradeLevels != null ? src.GradeLevels.Name : null))
                .ForMember(dest => dest.TeacherNames,
                    opt => opt.MapFrom(src =>
                        src.Exam != null && src.Exam.ExamGraders != null
                            ? src.Exam.ExamGraders.Select(eg => eg.User.FullName).ToList()
                            : new List<string>()))
       .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
        .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString())); ;
        }
    }
}
