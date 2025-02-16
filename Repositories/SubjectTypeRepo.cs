using System;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SubjectTypeRepo
    {
        private readonly isc_dbContext _context;
        public SubjectTypeRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<SubjectType> GetAllSubjectType()
        {
            return _context.SubjectTypes.ToList();
        }

        public SubjectType GetSubjectTypeById(long id)
        {
            return _context.SubjectTypes.FirstOrDefault(x => x.Id == id);
        }

        public SubjectType CreateSubjectType(SubjectType subjectType)
        {
            _context.SubjectTypes.Add(subjectType);
            _context.SaveChanges();
            return subjectType;
        }

        public SubjectType UpdateSubjectType(SubjectType subjectType)
        {
            _context.SubjectTypes.Update(subjectType);
            _context.SaveChanges();
            return subjectType;
        }

        public bool DeleteSubjectType(long id)
        {
            var subjectType = GetSubjectTypeById(id);
            if (subjectType != null)
            {
                _context.SubjectTypes.Remove(subjectType);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
