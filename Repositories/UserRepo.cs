using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class UserRepo
    {
        private readonly isc_dbContext _context;

        public UserRepo(isc_dbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các User
        public ICollection<User> GetUsers()
        {
            return _context.Users.Where( u => u.Active).ToList();
        }

        // Lấy User theo Id
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id && u.Active);
        }

        // Tạo mới một User
        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        // Cập nhật thông tin User
        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        // Xóa User
        public bool DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.Active = false;
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
