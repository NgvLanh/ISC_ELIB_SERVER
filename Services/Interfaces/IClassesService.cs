using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassesService
    {
        ApiResponse<ICollection<ClassesResponse>> GetClass(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ClassesResponse> GetClassById(long id);
        ApiResponse<ClassesResponse> GetClassByName(string name);
        ApiResponse<ClassesResponse> CreateClass(ClassesRequest classesRequest);
        ApiResponse<ClassesResponse> UpdateClass(long id, ClassesRequest classesRequest);
        ApiResponse<bool> DeleteClass(long id);
    }
}
