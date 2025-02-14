using System;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleClassRepo
    {
        private readonly isc_elibContext _context;

        public ExamScheduleClassRepo(isc_elibContext context)
        {
            _context = context;
        }

        public IEnumerable<ExamScheduleClass> GetAll()
        {
            return _context.ExamScheduleClasses.ToList();
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
