using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface ITeachingAssignmentsRepo
    {
        IQueryable<TeachingAssignment> GetTeachingAssignments();
        TeachingAssignment GetTeachingAssignmentById(int id);
        TeachingAssignment CreateTeachingAssignment(TeachingAssignment teachingAssignment);
        TeachingAssignment UpdateTeachingAssignment(TeachingAssignment teachingAssignment);
        bool DeleteTeachingAssignment(int id);
    }

    public class TeachingAssignmentsRepo : ITeachingAssignmentsRepo
    {
        private readonly isc_dbContext _context;

        public TeachingAssignmentsRepo(isc_dbContext context)
        {
            _context = context;
        }


        public TeachingAssignment GetTeachingAssignmentById(int id)
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

        public bool DeleteTeachingAssignment(int id)
        {
            var teachingAssignment = _context.TeachingAssignments.Find(id);

            if (teachingAssignment == null)
            {
                return false;
            }

            teachingAssignment.Active = false;
            _context.TeachingAssignments.Update(teachingAssignment);
            return _context.SaveChanges() > 0;
        }
        public IQueryable<TeachingAssignment> GetTeachingAssignments()
        {
            return _context.TeachingAssignments
                .Include(t => t.User)
                .Include(t => t.Class)
                .Include(t => t.Subject)
                .Include(t => t.Topics)
                .Include(t => t.Sessions);
        }
    }
}
