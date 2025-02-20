using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestsSubmissionRepo
    {
        private readonly isc_dbContext _context;
        public TestsSubmissionRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<TestsSubmission> GetTestsSubmissions()
        {
            return _context.TestsSubmissions.ToList();
        }

        public TestsSubmission GetTestsSubmissionById(long id)
        {
            return _context.TestsSubmissions.FirstOrDefault(s => s.Id == id);
        }

        public TestsSubmission CreateTestsSubmission(TestsSubmission TestsSubmission)
        {
            _context.TestsSubmissions.Add(TestsSubmission);
            _context.SaveChanges();
            return TestsSubmission;
        }

        public TestsSubmission UpdateTestsSubmission(TestsSubmission TestsSubmission)
        {
            _context.TestsSubmissions.Update(TestsSubmission);
            _context.SaveChanges();
            return TestsSubmission;
        }


        public bool DeleteTestsSubmission(long id)
        {
            var TestsSubmission = GetTestsSubmissionById(id);
            if (TestsSubmission != null)
            {
                _context.TestsSubmissions.Remove(TestsSubmission);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
