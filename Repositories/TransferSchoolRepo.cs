using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ISC_ELIB_SERVER.Models;
using System.Numerics;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;


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


        public TransferSchool CreateTransferSchool(TransferSchool entity)
        {

            try
            {
                _context.TransferSchools.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(" Lỗi khi lưu vào Database:");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                throw new Exception("Lưu dữ liệu thất bại! Chi tiết: " + ex.InnerException?.Message);
            }
           
        }


        /// <summary>
        /// Thêm mới thông tin chuyển trường.
        /// </summary>
        public TransferSchool? GetTransferSchoolById(int id)
        {
            return _context.TransferSchools
                .FirstOrDefault(ts => ts.Id == id && ts.Active);
        }


        /// <summary>
        /// Cập nhật thông tin chuyển trường.
        /// </summary>
        public TransferSchool? GetByStudentId(int studentId)
        {
            return _context.TransferSchools.FirstOrDefault(t => t.StudentId == studentId);
        }

        public TransferSchool UpdateTransferSchool(TransferSchool transferSchool)
        {
            try
            {
                _context.TransferSchools.Update(transferSchool);
                _context.SaveChanges();
                return transferSchool;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Lỗi khi cập nhật dữ liệu: {ex.InnerException?.Message}");
            }
        }


    }
}



