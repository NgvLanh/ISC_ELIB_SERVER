using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TestMapper : Profile
    {
        public TestMapper() {
            CreateMap<Test, TestResponse>();
            CreateMap<TestRequest, Test>();
        }
    }
}
