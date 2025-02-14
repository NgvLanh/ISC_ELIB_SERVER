using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class CampusMapper : Profile
    {
        public CampusMapper()
        {
            CreateMap<Campus, CampusResponse>();
            CreateMap<CampusRequest, Campus>();
        }
    }
}
