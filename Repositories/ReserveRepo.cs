using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ReserveRepo
    {
        private readonly isc_elibContext _context;
        public ReserveRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<Reserve> GetReserves()
        {
            return _context.Reserves.ToList();
        }

        public Reserve GetReserveById(long id)
        {
            return _context.Reserves.FirstOrDefault(r => r.Id == id);
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
            var reserve = GetReserveById(id);
            if (reserve != null)
            {
                _context.Reserves.Remove(reserve);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
