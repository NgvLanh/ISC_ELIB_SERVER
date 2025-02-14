using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamGraderRepo
    {
        private readonly isc_elibContext _context;

        public ExamGraderRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<ExamGrader> GetAll()
        {
            return _context.ExamGraders.ToList();
        }

        public ExamGrader? GetById(long id)
        {
            return _context.ExamGraders.FirstOrDefault(e => e.Id == id);
        }

        public ExamGrader Create(ExamGrader examGrader)
        {
            _context.ExamGraders.Add(examGrader);
            _context.SaveChanges();
            return examGrader;
        }

        public ExamGrader? Update(ExamGrader examGrader)
        {
            _context.ExamGraders.Update(examGrader);
            _context.SaveChanges();
            return examGrader;
        }

        public bool Delete(long id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.ExamGraders.Remove(entity);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
