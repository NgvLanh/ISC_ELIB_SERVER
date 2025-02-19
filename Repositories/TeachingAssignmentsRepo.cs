using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface ITeachingAssignmentsRepo
    {
        ICollection<TeachingAssignment> GetTeachingAssignments();
        TeachingAssignment GetTeachingAssignmentById(long id);
        TeachingAssignment CreateTeachingAssignment(TeachingAssignment teachingAssignment);
        TeachingAssignment UpdateTeachingAssignment(TeachingAssignment teachingAssignment);
        bool DeleteTeachingAssignment(long id);
    }

    public class TeachingAssignmentsRepo : ITeachingAssignmentsRepo
    {
        private readonly isc_dbContext _context;

        public TeachingAssignmentsRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<TeachingAssignment> GetTeachingAssignments()
        {
            return _context.TeachingAssignments.ToList();
        }

        public TeachingAssignment GetTeachingAssignmentById(long id)
        {
            return _context.TeachingAssignments.FirstOrDefault(t => t.Id == id);
        }

        public TeachingAssignment CreateTeachingAssignment(TeachingAssignment teachingAssignment)
        {
            _context.TeachingAssignments.Add(teachingAssignment);
            _context.SaveChanges();
            return teachingAssignment;
        }

        public TeachingAssignment? UpdateTeachingAssignment(TeachingAssignment teachingAssignment)
        {
            var existingAssignment = _context.TeachingAssignments.Find(teachingAssignment.Id);

            if (existingAssignment == null)
            {
                return null;
            }

            existingAssignment.UserId = teachingAssignment.UserId;
            existingAssignment.ClassId = teachingAssignment.ClassId;
            existingAssignment.SubjectId = teachingAssignment.SubjectId;
            existingAssignment.TopicsId = teachingAssignment.TopicsId;

            _context.SaveChanges();
            return existingAssignment;
        }

        public bool DeleteTeachingAssignment(long id)
        {
            var teachingAssignment = _context.TeachingAssignments.Find(id);

            if (teachingAssignment == null)
            {
                return false;
            }

            _context.TeachingAssignments.Remove(teachingAssignment);
            return _context.SaveChanges() > 0;
        }
    }
}
