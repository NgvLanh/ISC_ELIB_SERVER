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

        public PagedResult<ExamSchedule> GetAll(
     int page,
     int pageSize,
     string? search,
     string? sortBy,
     bool isDescending,
     int? academicYearId,
     int? semesterId,
     int? gradeLevelsId, 
     int? classId         
 )
        {
            var query = _context.ExamSchedules
      .Include(e => e.AcademicYear)
      .Include(e => e.SubjectNavigation)
      .Include(e => e.Semester)
      .Include(e => e.GradeLevels)
      .Include(e => e.Exam)
          .ThenInclude(ex => ex.ExamGraders)
              .ThenInclude(eg => eg.User)
      .Include(e => e.ExamScheduleClasses)  
          .ThenInclude(esc => esc.Class)   
      .Where(e => e.Active) 
      .AsNoTracking();
            if (academicYearId.HasValue)
            {
                query = query.Where(e => e.AcademicYear.Id == academicYearId.Value);
            }

            if (semesterId.HasValue)
            {
                query = query.Where(e => e.Semester.Id == semesterId.Value);
            }

            if (gradeLevelsId.HasValue)
            {
                query = query.Where(e => e.GradeLevels.Id == gradeLevelsId.Value);
            }

            if (classId.HasValue)
            {
                query = query.Where(e => e.ExamScheduleClasses.Any(c => c.ClassId == classId.Value));
            }

           
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));
            }

          
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }

            
            var totalCount = query.Count();

      
            var items = query.Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PagedResult<ExamSchedule>(items, totalCount, page, pageSize);
        }
        public ExamSchedule? GetById(long id)
        {
            var examSchedule = _context.ExamSchedules
                .Include(e => e.AcademicYear)
                .Include(e => e.SubjectNavigation)
                .Include(e => e.Semester)
                .Include(e => e.GradeLevels)
                .Include(e => e.Exam)
                    .ThenInclude(ex => ex.ExamGraders)
                        .ThenInclude(eg => eg.User)
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id && e.Active); 
            return examSchedule;
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

        public bool Delete(int id)
        {
            var entity = _context.ExamSchedules.Find(id);
            if (entity == null) return false;

            
            entity.Active = false;
            _context.SaveChanges();
            return true;
        }

    }
}
