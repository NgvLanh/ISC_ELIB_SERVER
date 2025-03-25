using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestRepo
    {
        private readonly isc_dbContext _context;
        public TestRepo(isc_dbContext context)
        {
            _context = context;
        }

        public IQueryable<Test> GetTests()
        {
                return _context.Tests
                    .Include(t => t.Subject)
                        .ThenInclude(s => s.SubjectGroup)
                    .Include(t => t.Subject)
                        .ThenInclude(s => s.SubjectType)
                    .Include(t => t.User)
                    .Include(t => t.GradeLevel);
        }


        public Test GetTestById(long id)
        {
            return _context.Tests.FirstOrDefault(s => s.Id == id);
        }

        public Test CreateTest(Test test)
        {
            _context.Tests.Add(test);
            _context.SaveChanges();
            return test;
        }

        public Test UpdateTest(Test test)
        {
            _context.Tests.Update(test);
            _context.SaveChanges();
            return test;
        }


        public bool DeleteTest(long id)
        {
            var test = GetTestById(id);
            if (test != null)
            {
                test.Active = false;
                _context.Tests.Update(test);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}

