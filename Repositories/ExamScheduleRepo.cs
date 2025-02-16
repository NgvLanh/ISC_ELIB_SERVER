using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleRepo
    {
        private readonly isc_dbContext _context;

        public ExamScheduleRepo(isc_dbContext context)
        {
            _context = context;
        }

        public PagedResult<ExamSchedule> GetAll(int page, int pageSize, string? search, string? sortBy, bool isDescending)
        {
            var query = _context.ExamSchedules.AsNoTracking();

            // 🔍 Tìm kiếm theo `Name` (hoặc có thể thay đổi)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search));
            }

            // 🔄 Sắp xếp động
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }

            // 📌 Tổng số bản ghi
            var totalCount = query.Count();

            // ⏳ Phân trang
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<ExamSchedule>(items, totalCount, page, pageSize);
        }

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
