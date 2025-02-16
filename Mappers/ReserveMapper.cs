using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
	public class ReserveMapper : Profile
	{
		public ReserveMapper()
		{
			// Reserve to ReserveResponse
			CreateMap<Reserve, ReserveResponse>();
			// ReserveRequest to Reserve
			CreateMap<ReserveRequest, Reserve>();
        }
    }
}
