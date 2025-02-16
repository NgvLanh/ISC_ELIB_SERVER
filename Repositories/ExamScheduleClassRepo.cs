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
            var query = _context.ExamScheduleClasses.AsQueryable();

            // 🔍 Tìm kiếm theo tên lịch thi
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Class.Name.Contains(searchTerm));
            }

            // 🔄 Sắp xếp dữ liệu
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isDescending = sortOrder?.ToLower() == "desc";

                query = sortBy.ToLower() switch
                {
                    "Id" => isDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                    _ => query.OrderBy(x => x.Id) // Mặc định sắp xếp theo Id
                };
            }

            int totalItems = query.Count(); // Tổng số bản ghi
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
