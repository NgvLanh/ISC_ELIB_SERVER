using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestAnswerRepo
    {
        private readonly isc_dbContext _context;
        public TestAnswerRepo(isc_dbContext context)
        {
            _context = context;
        }

        public bool IsCorrectAnswer(long answerId)
        {
            return _context.TestAnswers.Any(a => a.Id == answerId && a.IsCorrect == true);
        }
    }
}
