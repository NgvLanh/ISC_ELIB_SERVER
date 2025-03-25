using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class EntryTypeRepo
    {
        private readonly isc_dbContext _context;

        public EntryTypeRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<EntryType> GetEntryTypes()
        {
            return _context.EntryTypes
                .Where(c => c.Active)
                .ToList();
        }
        public EntryType GetEntryTypeById(long id)
        {
            return _context.EntryTypes.FirstOrDefault(e => e.Id == id && e.Active);
        }

        public EntryType CreateEntryType(EntryType entryType)
        {
            _context.EntryTypes.Add(entryType);
            _context.SaveChanges();
            return entryType;
        }

        public EntryType UpdateEntryType(EntryType entryType)
        {
            _context.EntryTypes.Update(entryType);
            _context.SaveChanges();
            return entryType;
        }
    }
}
