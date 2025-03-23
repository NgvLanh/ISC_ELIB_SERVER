using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class QuestionQaRepo
    {
        private readonly isc_dbContext _context;

        public QuestionQaRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<QuestionQa> GetQuestions()
        {
            return _context.QuestionQas.Include(q => q.Subject)
                                       .Include(q => q.AnswersQas)
                                       .Include(q => q.QuestionImagesQas)
                                       .ToList();
        }

        public QuestionQa GetQuestionById(long id)
        {
            return _context.QuestionQas.Include(q => q.Subject)
                                       .Include(q => q.AnswersQas)
                                       .Include(q => q.QuestionImagesQas)
                                       .FirstOrDefault(q => q.Id == id);
        }

        public QuestionQa CreateQuestion(QuestionQa question)
        {
            _context.QuestionQas.Add(question);
            _context.SaveChanges();
            return question;
        }

        public QuestionQa UpdateQuestion(QuestionQa question)
        {
            _context.QuestionQas.Update(question);
            _context.SaveChanges();
            return question;
        }

        public bool DeleteQuestion(long id)
        {
            var question = GetQuestionById(id);
            if (question != null)
            {
                _context.QuestionQas.Remove(question);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
