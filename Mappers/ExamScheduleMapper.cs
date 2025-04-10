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
            // Thêm mapping cho danh sách semester IDs
            .ForMember(dest => dest.SemesterIds,
                opt => opt.MapFrom(src =>
                    src.SemesterId.HasValue
                        ? new List<int> { src.SemesterId.Value }
                        : new List<int>()))

            // SemesterNames: từ navigation src.Semester.Name
            .ForMember(dest => dest.SemesterNames,
                opt => opt.MapFrom(src =>
                    src.Semester != null
                        ? new List<string> { src.Semester.Name }
                        : new List<string>()))

                .ForMember(dest => dest.GradeLevel,
                    opt => opt.MapFrom(src => src.GradeLevels != null ? src.GradeLevels.Name : null))
                .ForMember(dest => dest.TeacherNames,
                    opt => opt.MapFrom(src =>
                        src.Exam != null && src.Exam.ExamGraders != null
                            ? src.Exam.ExamGraders.Select(eg => eg.User.FullName).ToList()
                            : new List<string>()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status))
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Status.ToString()))
               .ForMember(dest => dest.duration_in_minutes, opt => opt.MapFrom(src => src.duration_in_minutes));


            CreateMap<ExamScheduleClass, ExamScheduleClassDetailDto>()
           .ForMember(dest => dest.ClassId,
               opt => opt.MapFrom(src => src.Class!.Id))
           .ForMember(dest => dest.ClassCode,
               opt => opt.MapFrom(src => src.Class!.Code))
           .ForMember(dest => dest.ClassName,
               opt => opt.MapFrom(src => src.Class!.Name))
           .ForMember(dest => dest.SupervisoryTeacherName,
               opt => opt.MapFrom(src =>
                   src.Class!.User != null
                       ? src.Class.User.FullName
                       : null))
           .ForMember(dest => dest.StudentQuantity,
               opt => opt.MapFrom(src => src.Class!.StudentQuantity ?? 0))
           .ForMember(dest => dest.JoinedExamStudentQuantity,
               opt => opt.MapFrom(src => src.joined_student_quantity))
           .ForMember(dest => dest.ExamGraders,
               opt => opt.MapFrom(src =>
                   src.ExamGraders != null
                       ? src.ExamGraders
                             .Where(eg => eg.User != null)
                             .Select(eg => eg.User.FullName)
                             .ToList()
                       : new List<string>()));
        }


    }
}
