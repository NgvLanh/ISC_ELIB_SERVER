﻿using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ISubjectService
    {
        ApiResponse<ICollection<SubjectResponse>> GetSubject(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<SubjectResponse> GetSubjectById(long id);
        ApiResponse<SubjectResponse> CreateSubject(SubjectRequest request);
        ApiResponse<SubjectResponse> UpdateSubject(long id, SubjectRequest request);
        ApiResponse<string> DeleteSubject(long id);
    }
}
