using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface ITeacherFamilyRepository
    {
        Task<IEnumerable<TeacherFamily>> GetAllAsync();
        Task<TeacherFamily?> GetByIdAsync(long id);
        Task AddAsync(TeacherFamily entity);
        Task UpdateAsync(TeacherFamily entity);
        Task DeleteAsync(long id);
    }
}
