using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IClassSubjectRepo
    {
        Task<List<ClassSubject>> GetClassSubjectsByClassIdAsync(int classId);
        Task<bool> AddClassSubjectsAsync(int classId, List<int> subjectIds);
        Task<bool> RemoveClassSubjectsByClassIdAsync(int classId, List<int> subjectIds);
        Task<bool> RemoveClassSubjectsByClassIdAsync(int classId); // Sửa trả về Task<bool>
    }

    public class ClassSubjectRepo : IClassSubjectRepo
    {
        private readonly isc_dbContext _context;

        public ClassSubjectRepo(isc_dbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassSubject>> GetClassSubjectsByClassIdAsync(int classId)
        {
            return await _context.ClassSubjects
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Subject)
                .ToListAsync();
        }

        public async Task<bool> AddClassSubjectsAsync(int classId, List<int> subjectIds)
        {
            try
            {
                var newClassSubjects = subjectIds.Select(subjectId => new ClassSubject
                {
                    ClassId = classId,
                    SubjectId = subjectId
                }).ToList();

                await _context.ClassSubjects.AddRangeAsync(newClassSubjects);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm môn học vào lớp: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveClassSubjectsByClassIdAsync(int classId, List<int> subjectIds)
        {
            try
            {
                var classSubjects = await _context.ClassSubjects
                    .Where(cs => cs.ClassId == classId && subjectIds.Contains(cs.Id)) // Sửa chỗ này
                    .ToListAsync();

                if (classSubjects.Any())
                {
                    _context.ClassSubjects.RemoveRange(classSubjects);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa môn học khỏi lớp: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveClassSubjectsByClassIdAsync(int classId)
        {
            try
            {
                var classSubjects = await _context.ClassSubjects
                    .Where(cs => cs.ClassId == classId)
                    .ToListAsync();

                if (classSubjects.Any())
                {
                    _context.ClassSubjects.RemoveRange(classSubjects);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa tất cả môn học của lớp: {ex.Message}");
                return false;
            }
        }
    }
}
