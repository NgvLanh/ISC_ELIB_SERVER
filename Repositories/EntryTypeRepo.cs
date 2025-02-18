namespace ISC_ELIB_SERVER.Repositories
{
    using ISC_ELIB_SERVER.Models;

    public class EntryTypeRepo
    {
        private readonly isc_elibContext _context;

        public EntryTypeRepo(isc_elibContext context)
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
            var existingEntryType = _context.EntryTypes.Find(entryType.Id);
            if (existingEntryType == null) return null;

            existingEntryType.Name = entryType.Name;
            _context.SaveChanges();
            return existingEntryType;
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
