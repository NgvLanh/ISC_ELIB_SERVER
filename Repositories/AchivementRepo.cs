using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AchivementRepo
    {
        private readonly isc_dbContext _context;
        public AchivementRepo(isc_dbContext context)
        {
            _context = context;
        }
        public ICollection<Achievement> GetAchievements()
        {
            return _context.Achievements.ToList();
        }

        public Achievement? GetAchivementById(int id)
        {

            return _context.Achievements.FirstOrDefault(s => s.Id == id);
        }

        public Achievement CreateAchivement(Achievement achivement)
        {
            _context.Achievements.Add(achivement);
            _context.SaveChanges();
            return achivement;
        }

        public Achievement? UpdateAchivement(Achievement updatedAchivement)
        {
            _context.Achievements.Update(updatedAchivement);
            _context.SaveChanges();

            return updatedAchivement;
        }


        public bool DeleteAchivement(int id)
        {
            var Achivement = GetAchivementById(id);
            if (Achivement != null)
            {
                _context.Achievements.Remove(Achivement);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
