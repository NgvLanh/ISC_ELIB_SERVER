using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // us - res
            CreateMap<UserStatus, UserStatusResponse>();
            // res - us
            CreateMap<UserStatusRequest, UserStatus>();
        }
    }
}
