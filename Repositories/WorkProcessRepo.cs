﻿using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Utils;

namespace ISC_ELIB_SERVER.Repositories
{
    public class WorkProcessRepo
    {
        private readonly isc_dbContext _context;
        public WorkProcessRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<WorkProcess> GetWorkProcess()
        {
            return _context.WorkProcesses.Where(wp => wp.Active).ToList();
        }

        public WorkProcess? GetWorkProcessById(long id)
        {
            return _context.WorkProcesses.FirstOrDefault(s => s.Id == id && s.Active);
        }

        public WorkProcess? GetWorkProcessByTeacherId(long id)
        {
            return _context.WorkProcesses.FirstOrDefault(s => s.TeacherId == id && s.Active);
        }

        public WorkProcess CreateWorkProcess(WorkProcess workProcess)
        {
            _context.WorkProcesses.Add(workProcess);
            _context.SaveChanges();
            return workProcess;
        }

        public WorkProcess? UpdateWorkProcess(long id, WorkProcessRequest workProcess)
        {
            var existingWorkProcess = GetWorkProcessById(id);

            if (existingWorkProcess == null)
            {
                return null;
            }
            existingWorkProcess.Organization = workProcess.Organization;
            existingWorkProcess.Position = workProcess.Position;
            existingWorkProcess.StartDate = workProcess.StartDate;
            existingWorkProcess.EndDate = workProcess.EndDate;
            existingWorkProcess.Program = workProcess.Program;
            existingWorkProcess.IsCurrent = workProcess.IsCurrent;
            existingWorkProcess.Active = workProcess.Active;
            _context.WorkProcesses.Update(existingWorkProcess);

            _context.SaveChanges();

            return existingWorkProcess;
        }

        public bool DeleteWorkProcess(long id)
        {
            var workProcess = GetWorkProcessById(id);
            if (workProcess != null)
            {
                workProcess.Active = false;
                _context.WorkProcesses.Update(workProcess);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
