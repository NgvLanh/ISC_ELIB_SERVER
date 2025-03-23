﻿using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AcademicYearRepo
    {
        private readonly isc_dbContext _context;
        public AcademicYearRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<AcademicYear> GetAcademicYears()
        {
            return _context.AcademicYears
                .Where(a => a.Active)
                .ToList();
        }

        public AcademicYear GetAcademicYearById(long id)
        {
            return _context.AcademicYears
                .Include(a => a.Semesters)
                .Where(a => a.Active)
                .FirstOrDefault(s => s.Id == id);
        }

        public bool IsDuplicateAcademicYear(long schoolId, DateTime startTime, DateTime endTime, long? excludeId = null)
        {
            return _context.AcademicYears.Any(a =>
                a.SchoolId == schoolId &&
                a.Active &&
                a.StartTime == startTime &&
                a.EndTime == endTime &&
                (!excludeId.HasValue || a.Id != excludeId.Value));
        }

        public AcademicYear CreateAcademicYear(AcademicYear academicYear)
        {
            if (IsDuplicateAcademicYear(academicYear.SchoolId ?? 0,
             academicYear.StartTime ?? DateTime.Now, academicYear.EndTime ?? DateTime.Now))
            {
                throw new Exception("Niên khóa này đã tồn tại trong trường.");
            }

            _context.AcademicYears.Add(academicYear);
            _context.SaveChanges();
            return academicYear;
        }

        public AcademicYear UpdateAcademicYear(AcademicYear academicYear)
        {
            if (IsDuplicateAcademicYear(academicYear.SchoolId ?? 0,
             academicYear.StartTime ?? DateTime.Now, academicYear.EndTime ?? DateTime.Now))
            {
                throw new Exception("Niên khóa này đã tồn tại trong trường.");
            }

            _context.AcademicYears.Update(academicYear);
            _context.SaveChanges();
            return academicYear;
        }

        public bool DeleteAcademicYear(long id)
        {
            var academicYear = GetAcademicYearById(id);
            if (academicYear != null)
            {
                academicYear.Active = false;
                _context.AcademicYears.Update(academicYear);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
