﻿using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TeachingAssignmentsMapper : Profile
    {
        public TeachingAssignmentsMapper()
        {
            CreateMap<TeachingAssignment, TeachingAssignmentsResponse>();
            CreateMap<TeachingAssignmentsRequest, TeachingAssignment>();
        }
    }
}
