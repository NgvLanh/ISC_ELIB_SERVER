using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TrainingProgramsRepo
    {
        private readonly isc_elibContext _context;
        public TrainingProgramsRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<TrainingProgram> GetTrainingProgram()
        {
            return _context.TrainingPrograms.ToList();
        }

        public TrainingProgram GetTrainingProgramById(long id)
        {
            return _context.TrainingPrograms.FirstOrDefault(s => s.Id == id);
        }

        public TrainingProgram CreateTrainingProgram(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Add(trainingProgram);
            _context.SaveChanges();
            return trainingProgram;
        }

        public TrainingProgram UpdateTrainingProgram(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Update(trainingProgram);
            _context.SaveChanges();
            return trainingProgram;
        }

        public bool DeleteTrainingProgram(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Update(trainingProgram);
            _context.SaveChanges();
            return true;
        }
    }
}
