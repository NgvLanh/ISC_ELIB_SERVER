using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ExamGraderRepository
{
    private readonly isc_dbContext _context;

    public ExamGraderRepository(isc_dbContext context)
    {
        _context = context;
    }

    public async Task<List<ExamGrader>> GetAllExamGradersAsync()
    {
        return await _context.ExamGraders.ToListAsync();
    }

    public async Task<ExamGrader?> GetExamGraderByIdAsync(int id)
    {
        return await _context.ExamGraders.FindAsync(id);
    }

    public async Task CreateExamGraderAsync(ExamGrader examGrader)
    {
        await _context.ExamGraders.AddAsync(examGrader);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExamGraderAsync(ExamGrader examGrader)
    {
        _context.ExamGraders.Update(examGrader);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteExamGraderAsync(int id)
    {
        var examGrader = await _context.ExamGraders.FindAsync(id);
        if (examGrader == null) return false;

        _context.ExamGraders.Remove(examGrader);
        await _context.SaveChangesAsync();
        return true;
    }
}
