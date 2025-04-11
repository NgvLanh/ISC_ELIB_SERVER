﻿using ISC_ELIB_SERVER.DTOs.Requests;
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
            return _context.Retirements.Where(r => r.Active).ToList();
        }

        public Retirement? GetRetirementById(long id)
        {
            return _context.Retirements.FirstOrDefault(r => r.Id == id && r.Active);
        }

        public ICollection<Retirement> GetRetirementByTeacherId(long id)
        {
            return _context.Retirements
                .Where(s => s.TeacherId == id)
                .ToList();
        }

        public Retirement GetRetirementByTeacherIdForPut(long id)
        {
            return _context.Retirements.FirstOrDefault(s => s.TeacherId == id && s.Active);
        }
        public Retirement CreateRetirement(Retirement retirement)
        {
            bool teacherExists = _context.TeacherInfos.Any(t => t.Id == retirement.TeacherId && t.Active == false);
            if (!teacherExists)
            {
                return null;
            }

            if (retirement.LeadershipId != 0)
            {
                bool leadershipExists = _context.Users.Any(u => u.Id == retirement.LeadershipId && u.Active == false);
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
            if (!_context.TeacherInfos.Any(t => t.Id == Retirement.TeacherId))
            {
                return null;
            }

            if (Retirement.LeadershipId != 0 && !_context.Users.Any(u => u.Id == Retirement.LeadershipId))
            {
                return null;
            }
            existingRetirement.TeacherId = Retirement.TeacherId;
            existingRetirement.Date = Retirement.Date;
            existingRetirement.Note = Retirement.Note;
            existingRetirement.Attachment = Retirement.Attachment;
            existingRetirement.Status = Retirement.Status;
            existingRetirement.Active = Retirement.Active;
            existingRetirement.LeadershipId = Retirement.LeadershipId;

            _context.Retirements.Update(existingRetirement);

            _context.SaveChanges();

            return existingRetirement;
        }

        public Retirement UpdateRetirementByTeacherId(long id, RetirementRequest Retirement)
        {
            var existingRetirement = GetRetirementByTeacherIdForPut(id);


            if (existingRetirement == null)
            {
                return null;
            }
            if (!_context.TeacherInfos.Any(t => t.Id == Retirement.TeacherId))
            {
                return null;
            }

            if (Retirement.LeadershipId != 0 && !_context.Users.Any(u => u.Id == Retirement.LeadershipId))
            {
                return null;
            }
            existingRetirement.TeacherId = Retirement.TeacherId;
            existingRetirement.Date = Retirement.Date;
            existingRetirement.Note = Retirement.Note;
            existingRetirement.Attachment = Retirement.Attachment;
            existingRetirement.Status = Retirement.Status;
            existingRetirement.Active = Retirement.Active;
            existingRetirement.LeadershipId = Retirement.LeadershipId;

            _context.Retirements.Update(existingRetirement);

            _context.SaveChanges();

            return existingRetirement;
        }

        public bool DeleteRetirement(long id)
        {
            var Retirement = GetRetirementById(id);
            if (Retirement != null)
            {
                Retirement.Active = false;
                _context.Retirements.Update(Retirement);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
