using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleRepo
    {
        private readonly isc_dbContext _context;

        public ExamScheduleRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<ExamSchedule> GetExamSchedules()
        {
            return _context.ExamSchedules.ToList();
        }

        public ExamSchedule GetExamScheduleById(int id)
        {
            return _context.ExamSchedules.FirstOrDefault(es => es.Id == id);
        }

        public ICollection<ExamSchedule> GetExamSchedulesByName(string name)
        {
            return _context.ExamSchedules
                .Where(es => es.Name.Contains(name))
                .ToList();
        }

        public ExamSchedule CreateExamSchedule(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Add(examSchedule);
            _context.SaveChanges();
            return examSchedule;
        }

        public ExamSchedule UpdateExamSchedule(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Update(examSchedule);
            _context.SaveChanges();
            return examSchedule;
        }

        public bool DeleteExamSchedule(int id)
        {
            var examSchedule = GetExamScheduleById(id);
            if (examSchedule != null)
            {
                // Xoá mềm: đổi trạng thái Active
                examSchedule.Active = !examSchedule.Active;
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
