using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleClassRepo
    {
        private readonly isc_dbContext _context;

        public ExamScheduleClassRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<ExamScheduleClass> GetExamScheduleClasses()
        {
            return _context.ExamScheduleClasses.ToList();
        }

        public ExamScheduleClass GetExamScheduleClassById(int id)
        {
            return _context.ExamScheduleClasses.FirstOrDefault(esc => esc.Id == id);
        }

        public ICollection<ExamScheduleClass> GetExamScheduleClassesByFilter(int? classId, int? exampleSchedule, int? supervisoryTeacherId)
        {
            var query = _context.ExamScheduleClasses.AsQueryable();

            if (classId.HasValue)
                query = query.Where(esc => esc.ClassId == classId);

            if (exampleSchedule.HasValue)
                query = query.Where(esc => esc.ExampleSchedule == exampleSchedule);

            if (supervisoryTeacherId.HasValue)
                query = query.Where(esc => esc.SupervisoryTeacherId == supervisoryTeacherId);

            return query.ToList();
        }

        public ExamScheduleClass CreateExamScheduleClass(ExamScheduleClass examScheduleClass)
        {
            _context.ExamScheduleClasses.Add(examScheduleClass);
            _context.SaveChanges();
            return examScheduleClass;
        }

        public ExamScheduleClass UpdateExamScheduleClass(ExamScheduleClass examScheduleClass)
        {
            _context.ExamScheduleClasses.Update(examScheduleClass);
            _context.SaveChanges();
            return examScheduleClass;
        }

        public bool DeleteExamScheduleClass(int id)
        {
            var examScheduleClass = GetExamScheduleClassById(id);
            if (examScheduleClass != null)
            {
                // Xoá mềm: thay đổi Active
                examScheduleClass.Active = !examScheduleClass.Active;
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public void Detach<T>(T entity) where T : class
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }
    }
}
