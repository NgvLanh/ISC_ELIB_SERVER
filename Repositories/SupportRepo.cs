using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SupportRepo
    {
        private readonly isc_elibContext _context;

        public SupportRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<Support> GetSupports()
        {
            return _context.Supports.ToList();
        }

        public Support GetSupportById(long id)
        {
            return _context.Supports.FirstOrDefault(n => n.Id == id);
        }

        public Support CreateSupport(Support Support)
        {
            _context.Supports.Add(Support);
            _context.SaveChanges();
            return Support;
        }

        public Support UpdateSupport(Support Support)
        {
            _context.Supports.Update(Support);
            _context.SaveChanges();
            return Support;
        }

        public bool DeleteSupport(long id)
        {
            var Support = GetSupportById(id);
            if (Support != null)
            {
                _context.Supports.Remove(Support);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
