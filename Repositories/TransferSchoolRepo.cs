using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISC_ELIB_SERVER.Models;
using System.Numerics;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TransferSchoolRepo
    {
        private readonly isc_dbContext _context;

        public TransferSchoolRepo(isc_dbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy danh sách học sinh đã chuyển trường.
        /// </summary>
        public List<object> GetTransferSchoolList()
        {
            return _context.TransferSchools
                .Where(ts => ts.Active)
                .Join(_context.StudentInfos, ts => ts.StudentId, si => si.Id, (ts, si) => new { ts, si })
                .Join(_context.Users, tsi => tsi.si.UserId, u => u.Id, (tsi, u) => new { tsi, u })
                .Join(_context.Semesters, tsu => tsu.tsi.ts.SemesterId, s => s.Id, (tsu, s) => new { tsu, s }) // Lấy từ khóa ngoại
                .Select(res => new
                {
                    StudentId = res.tsu.tsi.si.Id,
                    FullName = res.tsu.u.FullName,
                    DateOfBirth = res.tsu.u.Dob,
                    Gender = res.tsu.u.Gender == true ? "Nam" : "Nữ",
                    TransferDate = res.tsu.tsi.ts.TransferSchoolDate,
                    TransferSemester = res.s.Name,   // Hiển thị tên học kỳ
                    TransferToSchool = res.tsu.tsi.ts.TransferToSchool,
                    SemesterStart = res.s.StartTime, // Ngày bắt đầu học kỳ
                    SemesterEnd = res.s.EndTime      // Ngày kết thúc học kỳ
                })
                .Distinct()
                .ToList<object>();
        }


        /// <summary>
        /// Lấy thông tin chi tiết học sinh chuyển trường theo StudentId.
        /// </summary>
        public object? GetTransferSchoolByStudentId(int studentId)
        {
            return _context.TransferSchools
                .Where(ts => ts.StudentId == studentId && ts.Active)
                .Join(_context.StudentInfos, ts => ts.StudentId, si => si.Id, (ts, si) => new { ts, si })
                .Join(_context.Users, tsi => tsi.si.UserId, u => u.Id, (tsi, u) => new { tsi, u })
                .Join(_context.Semesters, tsu => tsu.tsi.ts.SemesterId, s => s.Id, (tsu, s) => new { tsu, s })
                .Select(res => new
                {
                    StudentCode = res.tsu.u.Code,       // Mã học viên
                    FullName = res.tsu.u.FullName,     // Tên học viên
                    TransferSemester = res.s.Name,     // Học kỳ chuyển trường
                    Province = res.tsu.u.ProvinceCode, // Tỉnh/Thành
                    District = res.tsu.u.DistrictCode, // Quận/Huyện
                    TransferFrom = res.tsu.tsi.ts.TransferToSchool, // Chuyển từ
                    Reason = res.tsu.tsi.ts.Reason,    // Lý do
                    Attachment = res.tsu.tsi.ts.AttachmentName // Tệp đính kèm
                })
                .FirstOrDefault(); // Lấy một bản ghi duy nhất hoặc trả về null nếu không có dữ liệu
        }


        public async Task<TransferSchoolResponse> CreateTransferSchoolAsync(TransferSchool_AddRequest request)
        {
            var studentInfo = await _context.StudentInfos
                .Where(s => s.Id == request.StudentId)
                .Include(s => s.User) // Bao gồm thông tin User để lấy ProvinceCode và DistrictCode
                .FirstOrDefaultAsync();

            if (studentInfo == null || studentInfo.User == null)
            {
                throw new Exception("Không tìm thấy thông tin học viên!");
            }

            var semester = await _context.Semesters
                .Where(s => s.Id == request.SemesterId)
                .FirstOrDefaultAsync();

            var transfer = new TransferSchool
            {
                StudentId = request.StudentId,
                TransferSchoolDate = request.TransferSchoolDate,
                TransferToSchool = request.TransferToSchool,
                SchoolAddress = request.SchoolAddress,
                Reason = request.Reason,
                AttachmentName = request.AttachmentName,
                AttachmentPath = request.AttachmentPath,
                SemesterId = request.SemesterId,
                // Lấy trực tiếp từ User
                LeadershipId = request.LeadershipId,
                Active = true
            };

            _context.TransferSchools.Add(transfer);
            await _context.SaveChangesAsync();

            return new TransferSchoolResponse
            {
                Id = transfer.Id,
                StudentName = studentInfo.User.FullName,
   
                TransferSchoolDate = transfer.TransferSchoolDate,
                TransferToSchool = transfer.TransferToSchool,
                SchoolAddress = transfer.SchoolAddress,
                Reason = transfer.Reason,
                AttachmentName = transfer.AttachmentName,
                AttachmentPath = transfer.AttachmentPath,
                SemesterName = semester?.Name,
            
                SemesterId = transfer.SemesterId,
                LeadershipId = transfer.LeadershipId
            };
        }


        /// <summary>
        /// Cập nhật thông tin chuyển trường.
        /// </summary>
        public object? UpdateTransferSchool(int transferSchoolId, TransferSchool updatedTransferSchool)
        {
            var transferSchool = _context.TransferSchools.FirstOrDefault(ts => ts.Id == transferSchoolId && ts.Active);
            if (transferSchool == null) return null;

            transferSchool.TransferSchoolDate = updatedTransferSchool.TransferSchoolDate;
            transferSchool.TransferToSchool = updatedTransferSchool.TransferToSchool;
            transferSchool.Reason = updatedTransferSchool.Reason;
            transferSchool.AttachmentName = updatedTransferSchool.AttachmentName;
            transferSchool.AttachmentPath = updatedTransferSchool.AttachmentPath;
            _context.SaveChanges();

            return _context.TransferSchools
                .Where(ts => ts.Id == transferSchool.Id)
                .Select(ts => new
                {
                    StudentId = ts.StudentId,
                    TransferDate = ts.TransferSchoolDate,
                    TransferToSchool = ts.TransferToSchool,
                    Reason = ts.Reason,
                    AttachmentName = ts.AttachmentName,
                    AttachmentPath = ts.AttachmentPath
                })
                .FirstOrDefault();
        }
    }
}
