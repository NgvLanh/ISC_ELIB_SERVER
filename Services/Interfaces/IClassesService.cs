using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassesService
    {
        ApiResponse<ICollection<ClassesResponse>> GetClass(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<ClassesResponse> GetClassById(int id);
        ApiResponse<ClassesResponse> GetClassByName(string name);
        ApiResponse<ClassesResponse> CreateClass(ClassesRequest classesRequest);
        ApiResponse<ClassesResponse> UpdateClass(int id, ClassesRequest classesRequest);
        ApiResponse<bool> DeleteClass(int id);
    }
}
