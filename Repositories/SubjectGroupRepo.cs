﻿using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SubjectGroupRepo
    {
        private readonly isc_elibContext _context;

        public SubjectGroupRepo(isc_elibContext context) { 
            _context = context;
        }

        public ICollection<SubjectGroup> GetAllSubjectGroup()
        {
            return _context.SubjectGroups.ToList();
        }

        public SubjectGroup GetSubjectGroupById(long id)
        {
            return _context.SubjectGroups.FirstOrDefault(x => x.Id == id);
        }

        public SubjectGroup CreateSubjectGroup(SubjectGroup subjectGroup)
        {
            _context.SubjectGroups.Add(subjectGroup);
            _context.SaveChanges();
            return subjectGroup;
        }

        public SubjectGroup UpdateSubjectGroup(SubjectGroup subjectGroup)
        {
            _context.SubjectGroups.Update(subjectGroup);
            _context.SaveChanges();
            return subjectGroup;
        }

        public bool DeleteSubjectGroup(long id)
        {
            var subjectGroup = GetSubjectGroupById(id);
            if (subjectGroup != null)
            {
                _context.SubjectGroups.Remove(subjectGroup);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
