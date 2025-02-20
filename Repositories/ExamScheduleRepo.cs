
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleRepository
    {
        private readonly isc_dbContext _context;

        public ExamScheduleRepository(isc_dbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExamSchedule>> GetAllAsync()
        {
            return await _context.ExamSchedules.ToListAsync();
        }

        public async Task<ExamSchedule?> GetByIdAsync(int id)
        {
            return await _context.ExamSchedules.FindAsync(id);
        }

        public async Task AddAsync(ExamSchedule examSchedule)
        {
            await _context.ExamSchedules.AddAsync(examSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Update(examSchedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Remove(examSchedule);
            await _context.SaveChangesAsync();
        }
    }
}
