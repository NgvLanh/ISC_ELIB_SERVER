using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TeacherFamilyRepository : ITeacherFamilyRepository
    {
        private readonly isc_elibContext _context;

        public TeacherFamilyRepository(isc_elibContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeacherFamily>> GetAllAsync()
        {
            return await _context.TeacherFamilies.ToListAsync();
        }

        public async Task<TeacherFamily?> GetByIdAsync(long id)
        {
            return await _context.TeacherFamilies.FindAsync(id);
        }

        public async Task<TeacherFamily> AddAsync(TeacherFamily entity)
        {
            await _context.TeacherFamilies.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(TeacherFamily entity)
        {
            var existingEntity = await _context.TeacherFamilies.FindAsync(entity.Id);
            if (existingEntity == null) throw new KeyNotFoundException("TeacherFamily not found");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;

            _context.TeacherFamilies.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        Task ITeacherFamilyRepository.AddAsync(TeacherFamily entity)
        {
            throw new NotImplementedException();
        }

        Task ITeacherFamilyRepository.DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }
    }


}
