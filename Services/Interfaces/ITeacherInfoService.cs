using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITeacherInfoService
    {
        ApiResponse<ICollection<TeacherInfoResponses>> GetTeacherInfos(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TeacherInfoResponses> GetTeacherInfoById(long id);
        ApiResponse<TeacherInfoResponses> GetTeacherInfoByCode(string code);
        ApiResponse<TeacherInfoResponses> CreateTeacherInfo(TeacherInfoRequest teacherInfoRequest);
        ApiResponse<TeacherInfoResponses> UpdateTeacherInfo(TeacherInfoRequest teacherInfoRequest);
        ApiResponse<TeacherInfoResponses> DeleteTeacherInfo(long id);
    }
}
