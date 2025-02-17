﻿using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class MojorMapper : Profile
    {
        public MojorMapper() {
            // us - res
            CreateMap<Major, MajorResponse>();
            // res - us
            CreateMap<MajorRequest, Major>();
        }
    }
}
