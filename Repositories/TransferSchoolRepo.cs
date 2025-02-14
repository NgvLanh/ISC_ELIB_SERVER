using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TransferSchoolRepo
    {
        private readonly isc_elibContext _context;
        public TransferSchoolRepo(isc_elibContext context)
        {
            _context = context;
        }

        public ICollection<TransferSchool> GetTransferSchools()
        {
            return _context.TransferSchools.Where(d => d.IsActive == true).ToList();
        }

        public TransferSchool GetTransferSchoolById(long id)
        {
            return _context.TransferSchools.FirstOrDefault(s => s.Id == id && s.IsActive == true);
        }

        public TransferSchool CreateTransferSchool(TransferSchool TransferSchool)
        {
            _context.TransferSchools.Add(TransferSchool);
            _context.SaveChanges();
            return TransferSchool;
        }

        public TransferSchool UpdateTransferSchool(long id, TransferSchool TransferSchool)
        {
            // Lấy đối tượng TransferSchool hiện có từ CSDL theo id
            var existingTransferSchool = _context.TransferSchools.FirstOrDefault(s => s.Id == id);

            if (existingTransferSchool == null)
            {
                // Xử lý trường hợp không tìm thấy, có thể ném exception hoặc trả về null
                return null;
            }


            existingTransferSchool.StudentId = TransferSchool.StudentId;
            existingTransferSchool.TransferSchoolDate = TransferSchool.TransferSchoolDate;
            existingTransferSchool.TransferToSchool = TransferSchool.TransferToSchool;
            existingTransferSchool.SchoolAddress = TransferSchool.SchoolAddress;
            existingTransferSchool.Reason = TransferSchool.Reason;
            existingTransferSchool.AttachmentName = TransferSchool.AttachmentName;
            existingTransferSchool.AttachmentPath = TransferSchool.AttachmentPath;
            existingTransferSchool.LeadershipId = TransferSchool.LeadershipId;
            existingTransferSchool.IsActive = TransferSchool.IsActive;

            // Lưu các thay đổi xuống CSDL
            _context.SaveChanges();

            return existingTransferSchool;
        }



        public bool DeleteTransferSchool(long id)
        {
            var TransferSchool = GetTransferSchoolById(id);
            if (TransferSchool != null)
            {
                _context.TransferSchools.Remove(TransferSchool);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
        public bool DeleteTransferSchool2(long id)
        {
            var TransferSchool = GetTransferSchoolById(id);
            if (TransferSchool != null)
            {
                TransferSchool.IsActive = false;
                _context.TransferSchools.Update(TransferSchool);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

    }
}
