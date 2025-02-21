﻿using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ISemesterService
    {
        ApiResponse<ICollection<SemesterResponse>> GetSemesters(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<SemesterResponse> GetSemesterById(long id);
        ApiResponse<SemesterResponse> CreateSemester(SemesterRequest SemesterRequest);
        ApiResponse<SemesterResponse> UpdateSemester(long id, SemesterRequest SemesterRequest);
        ApiResponse<object> DeleteSemester(long id);
    }
}
