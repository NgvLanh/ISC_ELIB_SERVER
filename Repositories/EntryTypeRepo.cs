using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
<<<<<<< HEAD
    public class EntryTypeRepo
    {
        private readonly isc_dbContext _context;

        public EntryTypeRepo(isc_dbContext context)
        {
            _context = context;
        }

=======

    public class EntryTypeRepo

    {

        private readonly isc_dbContext _context;
    public EntryTypeRepo(isc_dbContext context)
    {
        _context = context;
    }

    public ICollection<EntryType> GetEntryTypes()
        {
            return _context.EntryTypes.Where(et => !et.IsDeleted).ToList();
        }


>>>>>>> 30ea54130e2f90c7eb7720bf35ca70328b23fbb4
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
