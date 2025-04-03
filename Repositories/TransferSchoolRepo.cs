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
        private readonly GhnService _ghnService;
        public TransferSchoolRepo(isc_dbContext context, GhnService ghnService)
        {
            _context = context;
            _ghnService = ghnService;
        }

        /// <summary>
        /// Lấy danh sách học sinh đã chuyển trường.
        /// </summary>
        /// 

        public List<object> GetTransferSchoolList()
        {
            return _context.TransferSchools
            .Where(ts => ts.Active)
            .Join(_context.StudentInfos, ts => ts.StudentId, si => si.Id, (ts, si) => new { ts, si })
            .Join(_context.Users, tsi => tsi.si.UserId, u => u.Id, (tsi, u) => new { tsi, u })
            .Join(_context.Semesters, tsu => tsu.tsi.ts.SemesterId, s => s.Id, (tsu, s) => new { tsu, s }) // Lấy từ khóa ngoại
             .Join(_context.Classes, tsuc => tsuc.tsu.u.ClassId, c => c.Id, (tsuc, c) => new { tsuc, c }) // Liên kết với bảng Class qua ClassId trong bảng Users
            .Select(res => new
            {
                StudentId = res.tsuc.tsu.tsi.si.Id,
                FullName = res.tsuc.tsu.u.FullName,
                DateOfBirth = res.tsuc.tsu.u.Dob,
                Gender = res.tsuc.tsu.u.Gender == true ? "Nam" : "Nữ",
                TransferDate = res.tsuc.tsu.tsi.ts.TransferSchoolDate,
                TransferSemester = res.tsuc.s.Name,   // Hiển thị tên học kỳ
                TransferToSchool = res.tsuc.tsu.tsi.ts.TransferToSchool,
                GradeLevelId = res.c.GradeLevelId,   // Lấy GradeLevelId từ bảng Class
                SemesterStart = res.tsuc.s.StartTime, // Ngày bắt đầu học kỳ
                SemesterEnd = res.tsuc.s.EndTime     // Ngày kết thúc học kỳ
            })
            .Distinct()
            .ToList<object>();

        }


        /// <summary>
        /// Lấy thông tin chi tiết học sinh chuyển trường theo StudentId.
        /// </summary>
        public async Task<TransferSchoolResponse?> GetTransferSchoolByStudentId(int studentId)
        {
            var transferSchool = await _context.TransferSchools
                .Where(ts => ts.StudentId == studentId && ts.Active)  // Chỉ lọc các bản ghi chuyển trường còn hoạt động
                .Join(_context.StudentInfos, ts => ts.StudentId, si => si.Id, (ts, si) => new { ts, si })  // Kết hợp với bảng StudentInfos
                .Join(_context.Users, tsi => tsi.si.UserId, u => u.Id, (tsi, u) => new { tsi, u })  // Kết hợp với bảng Users
                .Join(_context.Semesters, tsu => tsu.tsi.ts.SemesterId, s => s.Id, (tsu, s) => new { tsu, s })  // Kết hợp với bảng Semesters
                .Select(res => new
                {
                    StudentCode = res.tsu.u.Code ?? "Không có mã học viên",  // Nếu null, gán giá trị mặc định
                    FullName = res.tsu.u.FullName ?? "Không có tên",  // Nếu null, gán giá trị mặc định
                    TransferSemester = res.s.Name ?? "Không có học kỳ",  // Nếu null, gán giá trị mặc định
                    TransferSchoolDate = res.tsu.tsi.ts.TransferSchoolDate ?? DateTime.MinValue,  // Nếu null, gán giá trị mặc định
                    TransferToSchool = res.tsu.tsi.ts.TransferToSchool ?? "Không có thông tin trường chuyển đến",  // Nếu null, gán giá trị mặc định
                    ProvinceCode = res.tsu.u.ProvinceCode ?? 0,  // Nếu null, gán giá trị mặc định
                    DistrictCode = res.tsu.u.DistrictCode ?? 0,  // Nếu null, gán giá trị mặc định
                    Reason = res.tsu.tsi.ts.Reason ?? "Không có lý do",  // Nếu null, gán giá trị mặc định
                    Attachment = res.tsu.tsi.ts.AttachmentName ?? "Không có tệp đính kèm"  // Nếu null, gán giá trị mặc định
                })
                .FirstOrDefaultAsync();  // Lấy bản ghi đầu tiên (hoặc null nếu không có dữ liệu)

            // Nếu không tìm thấy dữ liệu chuyển trường, trả về null
            if (transferSchool == null)
                return null;

            // Chuyển đổi mã tỉnh, quận thành tên
            var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(
                transferSchool.ProvinceCode,
                transferSchool.DistrictCode,
                "" // Không cần WardCode nữa
            );

            // Tạo đối tượng TransferSchoolResponse để trả về
            return new TransferSchoolResponse
            {
                FullName = transferSchool.FullName,  // Tên học viên
                StudentCode = transferSchool.StudentCode,  // Mã học viên
                TransferSemester = transferSchool.TransferSemester,  // Học kỳ chuyển trường
                TransferSchoolDate = transferSchool.TransferSchoolDate,  // Ngày chuyển trường
                TransferToSchool = transferSchool.TransferToSchool,  // Trường chuyển đến
                Reason = transferSchool.Reason,  // Lý do
                ProvinceName = provinceName,  // Tên tỉnh
                DistrictName = districtName,  // Tên quận
              
                AttachmentName = transferSchool.Attachment,  // Tên tệp đính kèm
                StatusCode = 200  // Trả về mã trạng thái 200
            };
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



