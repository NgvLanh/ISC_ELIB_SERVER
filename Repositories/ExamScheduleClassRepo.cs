using System;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleClassRepo
    {
        private readonly isc_dbContext _context;

        public ExamScheduleClassRepo(isc_dbContext context)
        {
            _context = context;
        }

        public PagedResult<ExamScheduleClass> GetAll(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder)
        {
            var query = _context.ExamScheduleClasses
                .Include(esc => esc.Class)
                .Include(esc => esc.SupervisoryTeacher)
                    .ThenInclude(t => t.User)
                .Include(esc => esc.ExampleScheduleNavigation)
                    .ThenInclude(es => es.Semester)
                .Include(esc => esc.ExampleScheduleNavigation)
                    .ThenInclude(es => es.GradeLevels)
                .Include(esc => esc.ExampleScheduleNavigation)
                    .ThenInclude(es => es.Exam) // Include Exam để lấy ExamGraders
                        .ThenInclude(e => e.ExamGraders)
                            .ThenInclude(eg => eg.User)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Class.Name.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isDescending = sortOrder?.ToLower() == "desc";
                query = sortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                    _ => query.OrderBy(x => x.Id)
                };
            }

            int totalItems = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<ExamScheduleClass>(items, totalItems, page, pageSize);
        }
        public ExamScheduleClass? GetById(long id)
        {
            return _context.ExamScheduleClasses.Find(id);
        }

        public void Create(ExamScheduleClass entity)
        {
            _context.ExamScheduleClasses.Add(entity);
            _context.SaveChanges();
        }

        public void Update(ExamScheduleClass entity)
        {
            _context.ExamScheduleClasses.Update(entity);
            _context.SaveChanges();
        }

        public bool Delete(long id)
        {
            var entity = _context.ExamScheduleClasses.Find(id);
            if (entity == null) return false;

            _context.ExamScheduleClasses.Remove(entity);
            _context.SaveChanges();
            return true;
        }
    }
}
