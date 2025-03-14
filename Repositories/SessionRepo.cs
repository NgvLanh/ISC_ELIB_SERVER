using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SessionRepo
    {
        private readonly isc_dbContext _context;
        public SessionRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Session> GetAllSessions()
        {
            return _context.Sessions.ToList();
        }

        public int Count()
        {
            return _context.Sessions.Count();
        }

        public Session GetSessionById(int id)
        {
            return _context.Sessions.FirstOrDefault(s => s.Id == id);
        }

        public Session CreateSession(Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
            return session;
        }

        public bool DeleteSession(int id)
        {
            var session = GetSessionById(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public Session UpdateSession(Session session)
        {
            _context.Sessions.Update(session);
            _context.SaveChanges();
            return session;
        }
    }
}
