using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ClassesMapper : Profile
    {
        public ClassesMapper()
        {
            // us - res
            CreateMap<Class, ClassesResponse>();
            // res - us
            CreateMap<ClassesRequest, Class>();
        }
    }
}
