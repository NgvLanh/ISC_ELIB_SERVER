using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ClassTypeMapper : Profile
    {
        public ClassTypeMapper()
        {
            // us - res
            CreateMap<ClassType, ClassTypeResponse>();
            // res - us
            CreateMap<ClassTypeRequest, ClassType>();
        }
    }
}
