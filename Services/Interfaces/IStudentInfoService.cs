using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IStudentInfoService
    {
        ApiResponse<ICollection<StudentInfoResponses>> GetStudentInfos(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<StudentInfoResponses> GetStudentInfoById(int id);
        ApiResponse<StudentInfoResponses> CreateStudentInfo(StudentInfoRequest studentInfoRequest);
        ApiResponse<StudentInfoResponses> UpdateStudentInfo(int id, StudentInfoRequest studentInfoRequest);
        ApiResponse<StudentInfoResponses> DeleteStudentInfo(int id);
    }
}
