using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
<<<<<<< HEAD
    public class EntryTypeRepo
=======
    using global::ISC_ELIB_SERVER.Models;

    namespace ISC_ELIB_SERVER.Repositories
>>>>>>> dev-2
    {
        private readonly isc_elibContext _context;

        public EntryTypeRepo(isc_elibContext context)
        {
<<<<<<< HEAD
            _context = context;
        }

        public ICollection<EntryType> GetEntryTypes()
        {
            return _context.EntryTypes.Where(et => !et.IsDeleted).ToList();
        }
=======
            private readonly isc_dbContext _context;

            public EntryTypeRepo(isc_dbContext context)
            {
                _context = context;
            }
>>>>>>> dev-2

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
