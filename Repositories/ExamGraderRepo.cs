using ISC_ELIB_SERVER.Data;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamGraderRepository
    {
        private readonly isc_dbContext _context;

        public ExamGraderRepository(isc_dbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamGrader>> GetAllAsync(int page, int pageSize, string? sortBy, bool isDescending, int? examId, int? userId)
        {
            var query = _context.ExamGraders.AsQueryable();

            if (examId.HasValue)
                query = query.Where(x => x.ExamId == examId);

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId);

            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isDescending ? query.OrderByDescending(x => EF.Property<object>(x, sortBy))
                                     : query.OrderBy(x => EF.Property<object>(x, sortBy));
            }

            return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<ExamGrader?> GetByIdAsync(int id)
        {
            return await _context.ExamGraders.FindAsync(id);
        }

        public async Task AddAsync(ExamGrader examGrader)
        {
            _context.ExamGraders.Add(examGrader);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ExamGrader examGrader)
        {
            _context.ExamGraders.Update(examGrader);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var examGrader = await _context.ExamGraders.FindAsync(id);
            if (examGrader != null)
            {
                _context.ExamGraders.Remove(examGrader);
                await _context.SaveChangesAsync();
            }
        }
    }
}
