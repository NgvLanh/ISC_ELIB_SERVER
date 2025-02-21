
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
public interface ISchoolService
{
    ApiResponse<ICollection<SchoolResponse>> GetSchools(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
    ApiResponse<SchoolResponse> GetSchoolById(long id);
    ApiResponse<SchoolResponse> CreateSchool(SchoolRequest schoolRequest);
    ApiResponse<SchoolResponse> UpdateSchool(long id, SchoolRequest schoolRequest);
    ApiResponse<object> DeleteSchool(long id);
}
