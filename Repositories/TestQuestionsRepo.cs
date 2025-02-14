﻿using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestQuestionRepo
    {
        private readonly isc_elibContext _context;
        public TestQuestionRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<TestQuestion> GetTestQuestions()
        {
            return _context.TestQuestions.ToList();
        }

        public TestQuestion GetTestQuestionById(long id)
        {
            return _context.TestQuestions.FirstOrDefault(s => s.Id == id);
        }

        public TestQuestion CreateTestQuestion(TestQuestion TestQuestion)
        {
            _context.TestQuestions.Add(TestQuestion);
            _context.SaveChanges();
            return TestQuestion;
        }

        public TestQuestion UpdateTestQuestion(TestQuestion TestQuestion)
        {
            _context.TestQuestions.Update(TestQuestion);
            _context.SaveChanges();
            return TestQuestion;
        }


        public bool DeleteTestQuestion(long id)
        {
            var TestQuestion = GetTestQuestionById(id);
            if (TestQuestion != null)
            {
                _context.TestQuestions.Remove(TestQuestion);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}

