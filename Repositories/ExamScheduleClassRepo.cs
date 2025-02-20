using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Repository
{
    public class ExamScheduleClassRepository
    {
        private readonly isc_dbContext _context;

        public ExamScheduleClassRepository(isc_dbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExamScheduleClass>> GetAllAsync()
        {
            return await _context.ExamScheduleClasses.ToListAsync();
        }

        public async Task<ExamScheduleClass?> GetByIdAsync(int id)
        {
            return await _context.ExamScheduleClasses.FindAsync(id);
        }

        public async Task<ExamScheduleClass> AddAsync(ExamScheduleClass examScheduleClass)
        {
            _context.ExamScheduleClasses.Add(examScheduleClass);
            await _context.SaveChangesAsync();
            return examScheduleClass;
        }

        public async Task<ExamScheduleClass> UpdateAsync(ExamScheduleClass examScheduleClass)
        {
            _context.ExamScheduleClasses.Update(examScheduleClass);
            await _context.SaveChangesAsync();
            return examScheduleClass;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ExamScheduleClasses.FindAsync(id);
            if (entity == null) return false;

            _context.ExamScheduleClasses.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
