using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TransferSchoolMapper : Profile
    {
        public TransferSchoolMapper()
        {
            CreateMap<TransferSchool, TransferSchool_AddRequest>().ReverseMap();
            CreateMap<TransferSchool, TransferSchool_UpdateRequest>().ReverseMap();

            CreateMap<TransferSchoolResponse, TransferSchool>().ReverseMap();
        }
    }
}
