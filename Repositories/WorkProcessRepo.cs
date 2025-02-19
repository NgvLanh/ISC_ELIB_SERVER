using ISC_ELIB_SERVER.DTOs.Requests;
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
            return _context.WorkProcesses.ToList();
        }

        public WorkProcess GetWorkProcessById(long id)
        {
            return _context.WorkProcesses.FirstOrDefault(s => s.Id == id);
        }

        public WorkProcess CreateWorkProcess(WorkProcess WorkProcess)
        {
            _context.WorkProcesses.Add(WorkProcess);
            _context.SaveChanges();
            return WorkProcess;
        }

        public WorkProcess UpdateWorkProcess(long id, WorkProcessRequest workProcess)
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
            existingWorkProcess.IsCurrent = workProcess.IsCurrent;
            existingWorkProcess.Active = workProcess.Active;
            _context.WorkProcesses.Update(existingWorkProcess);

            _context.SaveChanges();

            return existingWorkProcess;
        }


        public bool DeleteWorkProcess(long id)
        {
            var WorkProcess = GetWorkProcessById(id);
            if (WorkProcess != null)
            {
                _context.WorkProcesses.Remove(WorkProcess);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
