using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassesService
    {
        ApiResponse<ICollection<ClassesResponse>> GetClass(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<ICollection<ClassesResponse>> GetClassByGradeLevelId(int? page, int? pageSize, int? gradeLevelId, string? sortColumn, string? sortOrder);
        ApiResponse<ClassesResponse> GetClassById(int id);
        ApiResponse<bool> DeleteClass(List<int> ids);

        Task<ApiResponse<bool>> UpdateClassSubjectsAsync(int classId, List<int> subjectIds);
        Task<ApiResponse<ClassesResponse>> CreateClassAsync(ClassesRequest classesRequest);
        Task<ApiResponse<ClassesResponse>> UpdateClassAsync(int id, ClassesRequest classesRequest);

        Task<ApiResponse<bool>> ImportClassesAsync(IFormFile file);

    }
}
