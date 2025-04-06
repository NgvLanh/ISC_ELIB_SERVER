using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class StudentInfoMapper : Profile
    {
        public StudentInfoMapper()
        {
            // Mapping từ DTO Request -> Entity
            CreateMap<StudentInfoRequest, StudentInfo>();

            // Mapping từ Entity -> Response cơ bản
            CreateMap<StudentInfo, StudentInfoResponses>();

            // Mapping từ StudentInfo -> StudentInfoUserResponse (lấy thêm thông tin từ User, Class, UserStatus)
            CreateMap<StudentInfo, StudentInfoUserResponse>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User != null ? src.User.Id : (int?)null))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.User != null ? src.User.Code : null))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.User != null ? src.User.Dob : (DateTime?)null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User != null ? (src.User.Gender == true ? "Nam" : "Nữ") : null))
                .ForMember(dest => dest.Nation, opt => opt.MapFrom(src => src.User != null ? src.User.Nation : null))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.User != null && src.User.Class != null ? src.User.Class.Name : null))
                .ForMember(dest => dest.status, opt => opt.MapFrom(src => src.User != null && src.User.UserStatus != null ? src.User.UserStatus.Name : null))
                // Lấy thông tin AcademicYearId và Semesters từ User -> AcademicYear
                .ForMember(dest => dest.AcademicYearId, opt => opt.MapFrom(src => src.User != null && src.User.AcademicYear != null ? src.User.AcademicYear.Id : (int?)null))
                .ForMember(dest => dest.Semesters, opt => opt.MapFrom(src => src.User != null && src.User.AcademicYear != null ?
                    src.User.AcademicYear.Semesters.Select(se => new SemesterResponse
                    {
                        Id = se.Id,
                        Name = se.Name,
                        StartTime = se.StartTime ?? default(DateTime),
                        EndTime = se.EndTime ?? default(DateTime),
                        AcademicYearId = se.AcademicYearId.HasValue ? se.AcademicYearId.Value : 0
                    }).ToList() : new List<SemesterResponse>())
                );

            // Mapping từ StudentInfo -> StudentInfoClassResponse (lấy thêm thông tin từ User, AcademicYear, UserStatus)
            CreateMap<StudentInfo, StudentInfoClassResponse>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.User != null ? src.User.Code : null))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                .ForMember(dest => dest.AcademicYear, opt => opt.MapFrom(src =>
                    src.User != null && src.User.AcademicYear != null
                    ? src.User.AcademicYear.StartTime.HasValue && src.User.AcademicYear.EndTime.HasValue
                        ? src.User.AcademicYear.StartTime.Value.Year + "-" + src.User.AcademicYear.EndTime.Value.Year
                        : null
                    : null))
                .ForMember(dest => dest.EnrollmentDate, opt => opt.MapFrom(src => src.User != null ? src.User.EnrollmentDate : (DateTime?)null))
                .ForMember(dest => dest.UserStatusName, opt => opt.MapFrom(src => src.User != null && src.User.UserStatus != null ? src.User.UserStatus.Name : null));
        }
    }
}
