using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestAnswerRepo
    {
        private readonly isc_elibContext _context;
        public TestAnswerRepo(isc_elibContext context)
        {
            _context = context;
        }

        public bool IsCorrectAnswer(long answerId)
        {
            return _context.TestAnswers.Any(a => a.Id == answerId && a.IsCorrect == true);
        }
    }
}
