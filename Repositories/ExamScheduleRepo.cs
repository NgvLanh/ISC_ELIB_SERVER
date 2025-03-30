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

        public PagedResult<ExamSchedule> GetAll(int page, int pageSize, string? search, string? sortBy, bool isDescending, int? academicYearId, int? semesterId)
        {
            var query = _context.ExamSchedules
                .Include(e => e.AcademicYear)
                .Include(e => e.SubjectNavigation)
                .Include(e => e.Semester)
                .Include(e => e.GradeLevels)
                .Include(e => e.Exam)
                    .ThenInclude(ex => ex.ExamGraders)
                        .ThenInclude(eg => eg.User)
                .AsNoTracking();

            // 🔍 Lọc theo `academicYearId` nếu có
            if (academicYearId.HasValue)
            {
                query = query.Where(e => e.AcademicYear.Id == academicYearId.Value);
            }

            // 🔍 Lọc theo `semesterId` nếu có
            if (semesterId.HasValue)
            {
                query = query.Where(e => e.Semester.Id == semesterId.Value);
            }

            // 🔍 Tìm kiếm theo Name
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));
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
            var items = query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            return new PagedResult<ExamSchedule>(items, totalCount, page, pageSize);
        }


        public ExamSchedule? GetById(long id)
        {
            return _context.ExamSchedules
                .Include(e => e.AcademicYear)
                .Include(e => e.SubjectNavigation)
                .Include(e => e.Semester)
                .Include(e => e.GradeLevels)
                .FirstOrDefault(e => e.Id == id);
        }

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
