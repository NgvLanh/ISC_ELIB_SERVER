using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AnswersQaRepo
    {
        private readonly isc_dbContext _context;

        public AnswersQaRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<AnswersQa> GetAnswers()
        {
            return _context.AnswersQas.Include(a => a.Question)
                                      .Include(a => a.AnswerImagesQas)
                                      .ToList();
        }

        public AnswersQa GetAnswerById(long id)
        {
            return _context.AnswersQas.Include(a => a.Question)
                                      .Include(a => a.AnswerImagesQas)
                                      .FirstOrDefault(a => a.Id == id);
        }

        public AnswersQa CreateAnswer(AnswersQa answer)
        {
            _context.AnswersQas.Add(answer);
            _context.SaveChanges();
            return answer;
        }

        public AnswersQa UpdateAnswer(AnswersQa answer)
        {
            _context.AnswersQas.Update(answer);
            _context.SaveChanges();
            return answer;
        }

        public bool DeleteAnswer(long id)
        {
            var answer = GetAnswerById(id);
            if (answer != null)
            {
                _context.AnswersQas.Remove(answer);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
