using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class RetirementRepo
    {
        private readonly isc_dbContext _context;
        public RetirementRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<Retirement> GetRetirement()
        {
            return _context.Retirements.ToList();
        }

        public Retirement GetRetirementById(long id)
        {
            return _context.Retirements.FirstOrDefault(s => s.Id == id);
        }

        public ICollection<Retirement> GetRetirementByTeacherId(long id)
        {
            return _context.Retirements
                .Where(s => s.TeacherId == id)
                .ToList();
        }

        public Retirement CreateRetirement(Retirement retirement)
        {
            bool teacherExists = _context.TeacherInfos.Any(t => t.Id == retirement.TeacherId);
            if (!teacherExists)
            {
                return null;
            }

            if (retirement.LeadershipId.HasValue)
            {
                bool leadershipExists = _context.Users.Any(u => u.Id == retirement.LeadershipId.Value);
                if (!leadershipExists)
                {
                    return null;
                }
            }

            _context.Retirements.Add(retirement);
            _context.SaveChanges();

            return retirement;
        }


        public Retirement UpdateRetirement(long id, RetirementRequest Retirement)
        {
            var existingRetirement = GetRetirementById(id);


            if (existingRetirement == null)
            {
                return null;
            }
            existingRetirement.Date = Retirement.Date;
            existingRetirement.Note = Retirement.Note;
            existingRetirement.Attachment = Retirement.Attachment;
            existingRetirement.Status = Retirement.Status;
            existingRetirement.Active = Retirement.Active;
            _context.Retirements.Update(existingRetirement);

            _context.SaveChanges();

            return existingRetirement;
        }


        public bool DeleteRetirement(long id)
        {
            var Retirement = GetRetirementById(id);
            if (Retirement != null)
            {
                _context.Retirements.Remove(Retirement);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
