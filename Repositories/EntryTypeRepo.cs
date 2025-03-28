namespace ISC_ELIB_SERVER.Repositories
{
    using global::ISC_ELIB_SERVER.Models;

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
                return _context.EntryTypes.ToList();
            }

            public EntryType GetEntryTypeById(long id)
            {
                return _context.EntryTypes.FirstOrDefault(e => e.Id == id);
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

            public bool DeleteEntryType(long id)
            {
                var entryType = GetEntryTypeById(id);
                if (entryType != null)
                {
                    _context.EntryTypes.Remove(entryType);
                    return _context.SaveChanges() > 0;
                }
                return false;
            }
        }
    }

}
