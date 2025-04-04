using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class SubjectMapper: Profile
    {
        public SubjectMapper() {
            CreateMap<SubjectSubjectGroup, SubjectGroupResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubjectGroup.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SubjectGroup.Name))
                .ForMember(dest => dest.TeacherId, opt => opt.MapFrom(src => src.SubjectGroup.TeacherId));
                

            CreateMap<Subject, SubjectResponse>()
                .AfterMap((src, dest, context) =>
                {
                    dest.SubjectGroup = context.Mapper.Map<List<SubjectGroupResponse>>(src.SubjectSubjectGroups);
                    dest.SubjectType = context.Mapper.Map<SubjectTypeResponse>(src.SubjectType);
                });
            CreateMap<SubjectRequest, Subject>();
        }
    }
}
