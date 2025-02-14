using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IScoreTypeRepo
    {
        ICollection<ScoreType> GetScoreTypes();
        ScoreType GetScoreTypeById(long id);
        ScoreType CreateScoreType(ScoreType scoreType);
        ScoreType UpdateScoreType(ScoreType scoreType);
        bool DeleteScoreType(long id);
    }

    public class ScoreTypeRepo : IScoreTypeRepo
    {
        private readonly isc_elibContext _context;

        public ScoreTypeRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<ScoreType> GetScoreTypes()
        {
            return _context.ScoreTypes.ToList();
        }

        public ScoreType GetScoreTypeById(long id)
        {
            return _context.ScoreTypes.FirstOrDefault(s => s.Id == id);
        }

        public ScoreType CreateScoreType(ScoreType scoreType)
        {
            _context.ScoreTypes.Add(scoreType);
            _context.SaveChanges();
            return scoreType;
        }

        public ScoreType? UpdateScoreType(ScoreType scoreType)
        {
            var existingScoreType = _context.ScoreTypes.Find(scoreType.Id);

            if (existingScoreType == null)
            {
                return null;
            }

            existingScoreType.Name = scoreType.Name;
            existingScoreType.Weight = scoreType.Weight;
            existingScoreType.QtyScoreSemester1 = scoreType.QtyScoreSemester1;
            existingScoreType.QtyScoreSemester2 = scoreType.QtyScoreSemester2;

            _context.SaveChanges();
            return existingScoreType;
        }


        public bool DeleteScoreType(long id)
        {
            var scoreType = _context.ScoreTypes.Find(id);

            if (scoreType == null)
            {
                return false;
            }

            _context.ScoreTypes.Remove(scoreType);
            return _context.SaveChanges() > 0;
        }

    }
}
