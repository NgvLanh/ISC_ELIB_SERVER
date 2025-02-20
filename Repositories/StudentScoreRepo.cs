using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IStudentScoreRepo
    {
        ICollection<StudentScore> GetStudentScores();
        StudentScore GetStudentScoreById(long id);
        StudentScore CreateStudentScore(StudentScore studentScore);
        StudentScore UpdateStudentScore(StudentScore studentScore);
        bool DeleteStudentScore(long id);
    }

    public class StudentScoreRepo : IStudentScoreRepo
    {
        private readonly isc_dbContext _context;

        public StudentScoreRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<StudentScore> GetStudentScores()
        {
            return _context.StudentScores.ToList();
        }

        public StudentScore GetStudentScoreById(long id)
        {
            return _context.StudentScores.FirstOrDefault(s => s.Id == id);
        }

        public StudentScore CreateStudentScore(StudentScore studentScore)
        {
            _context.StudentScores.Add(studentScore);
            _context.SaveChanges();
            return studentScore;
        }

        public StudentScore? UpdateStudentScore(StudentScore studentScore)
        {
            var existingStudentScore = _context.StudentScores.Find(studentScore.Id);

            if (existingStudentScore == null)
            {
                return null;
            }

            existingStudentScore.UserId = studentScore.UserId;
            existingStudentScore.ScoreTypeId = studentScore.ScoreTypeId;
            existingStudentScore.SubjectId = studentScore.SubjectId;
            existingStudentScore.SemesterId = studentScore.SemesterId;

            _context.SaveChanges();
            return existingStudentScore;
        }

        public bool DeleteStudentScore(long id)
        {
            var studentScore = _context.StudentScores.Find(id);

            if (studentScore == null)
            {
                return false;
            }

            _context.StudentScores.Remove(studentScore);
            return _context.SaveChanges() > 0;
        }
    }
}
