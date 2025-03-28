using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ReserveRepo
    {
        private readonly isc_dbContext _context;
        public ReserveRepo(isc_dbContext context)
        {
            _context = context;
        }

        public Reserve? GetReserveById(long id)
        {
            return _context.Reserves
                .Include(r => r.Student)  // Load thông tin User (Student)
                .Include(r => r.Class)    // Load thông tin Class
                .FirstOrDefault(r => r.Id == id); // Tìm theo Reserve.Id
        }

        public Reserve? GetReserveByStudentId(int studentId)
        {
            return _context.Reserves
                .Include(r => r.Student)
                .Include(r => r.Class)
                .FirstOrDefault(r => r.StudentId == studentId); // Tìm theo StudentId (User.Id)
        }


        public Reserve CreateReserve(Reserve reserve)
        {
            _context.Reserves.Add(reserve);
            _context.SaveChanges();
            return reserve;
        }

        public Reserve UpdateReserve(Reserve reserve)
        {
            _context.Reserves.Update(reserve);
            _context.SaveChanges();
            return reserve;
        }

        public bool DeleteReserve(long id)
        {
            var reserve = _context.Reserves.FirstOrDefault(r => r.Id == id);
            if (reserve != null)
            {
                reserve.Active = false;
                _context.Reserves.Update(reserve); // Đánh dấu entity đã thay đổi
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public ICollection<Reserve> GetActiveReserves(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _context.Reserves
                .Include(r => r.Student)
                .Include(r => r.Class)
                .Where(r => r.Active == true);

            // Tìm kiếm theo tên học viên
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(r => r.Student.FullName.Contains(search));
            }

            // Sắp xếp theo cột được chọn
            switch (sortColumn?.ToLower())
            {
                case "fullname":
                    query = sortOrder == "desc" ? query.OrderByDescending(r => r.Student.FullName) : query.OrderBy(r => r.Student.FullName);
                    break;
                case "reservedate":
                    query = sortOrder == "desc" ? query.OrderByDescending(r => r.ReserveDate) : query.OrderBy(r => r.ReserveDate);
                    break;
                default:
                    query = query.OrderBy(r => r.Id);
                    break;
            }

            // Áp dụng phân trang
            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

    }
}
