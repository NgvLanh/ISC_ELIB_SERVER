using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class EntryTypeRepo
    {
        private readonly isc_elibContext _context;

        public EntryTypeRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<EntryType> GetEntryTypes()
        {
            return _context.EntryTypes.Where(et => !et.IsDeleted).ToList();
        }

        public EntryType GetEntryTypeById(long id)
        {
            return _context.EntryTypes.FirstOrDefault(e => e.Id == id && !e.IsDeleted);
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
