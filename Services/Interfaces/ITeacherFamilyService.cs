using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface ITeacherFamilyService
    {
        Task<IEnumerable<TeacherFamilyResponse>> GetAllAsync();
        Task<TeacherFamilyResponse?> GetByIdAsync(long id);
        Task AddAsync(TeacherFamilyRequest request);
        Task UpdateAsync(long id, TeacherFamilyRequest request);
        Task DeleteAsync(long id);
    }
}
