using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassesService
    {
        ApiResponse<ICollection<ClassesResponse>> GetClass(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<ClassesResponse> GetClassById(int id);
        ApiResponse<ClassesResponse> GetClassByName(string name);
        ApiResponse<bool> DeleteClass(int id);

        Task<ApiResponse<bool>> UpdateClassSubjectsAsync(int classId, List<int> subjectIds);
        Task<ApiResponse<ClassesResponse>> CreateClassAsync(ClassesRequest classesRequest);
        Task<ApiResponse<ClassesResponse>> UpdateClassAsync(int id, ClassesRequest classesRequest);
    }
}
