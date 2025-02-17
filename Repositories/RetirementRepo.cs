﻿using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class RetirementRepo
    {
        private readonly isc_elibContext _context;
        public RetirementRepo(isc_elibContext context)
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

        public Retirement CreateRetirement(Retirement Retirement)
        {
            _context.Retirements.Add(Retirement);
            _context.SaveChanges();
            return Retirement;
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
