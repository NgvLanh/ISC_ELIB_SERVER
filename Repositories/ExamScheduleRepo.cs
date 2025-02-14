using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleRepo
    {
        private readonly isc_elibContext _context;

        public ExamScheduleRepo(isc_elibContext context)
        {
            _context = context;
        }

        public IEnumerable<ExamSchedule> GetAll() => _context.ExamSchedules.AsNoTracking().ToList();

        public ExamSchedule? GetById(long id) => _context.ExamSchedules.Find(id);

        public void Create(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Add(examSchedule);
            _context.SaveChanges();
        }

        public void Update(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Update(examSchedule);
            _context.SaveChanges();
        }

        public bool Delete(long id)
        {
            var entity = _context.ExamSchedules.Find(id);
            if (entity == null) return false;

            _context.ExamSchedules.Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}
