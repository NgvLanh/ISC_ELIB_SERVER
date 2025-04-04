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
            .Join(_context.Semesters, tsu => tsu.tsi.ts.SemesterId, s => s.Id, (tsu, s) => new { tsu, s })
            .Join(_context.Classes, tsuc => tsuc.tsu.u.ClassId, c => c.Id, (tsuc, c) => new { tsuc, c })
            .Join(_context.GradeLevels, tsucg => tsucg.c.GradeLevelId, gl => gl.Id, (tsucg, gl) => new { tsucg, gl }) // JOIN với bảng GradeLevels

            .Select(res => new
            {
                StudentId = res.tsucg.tsuc.tsu.tsi.si.Id,
                FullName = res.tsucg.tsuc.tsu.u.FullName,
                DateOfBirth = res.tsucg.tsuc.tsu.u.Dob,
                Gender = res.tsucg.tsuc.tsu.u.Gender == true ? "Nam" : "Nữ",
                TransferDate = res.tsucg.tsuc.tsu.tsi.ts.TransferSchoolDate,
                TransferSemester = res.tsucg.tsuc.s.Name,
                TransferToSchool = res.tsucg.tsuc.tsu.tsi.ts.TransferToSchool,
                GradeLevel = res.gl.Name,   // Lấy tên của cấp lớp thay vì Id
                SemesterStart = res.tsucg.tsuc.s.StartTime,
                SemesterEnd = res.tsucg.tsuc.s.EndTime
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
                .Where(ts => ts.StudentId == studentId && ts.Active)
                .Join(_context.StudentInfos, ts => ts.StudentId, si => si.Id, (ts, si) => new { ts, si })
                .Join(_context.Users, tsi => tsi.si.UserId, u => u.Id, (tsi, u) => new { tsi, u })
                .Join(_context.Semesters, tsu => tsu.tsi.ts.SemesterId, s => s.Id, (tsu, s) => new { tsu, s })
                .Select(res => new
                {
                    StudentCode = res.tsu.u.Code ?? "Không có mã học viên",
                    FullName = res.tsu.u.FullName ?? "Không có tên",
                    TransferSemester = res.s.Name ?? "Không có học kỳ",
                    TransferSchoolDate = res.tsu.tsi.ts.TransferSchoolDate ?? DateTime.MinValue,
                    TransferToSchool = res.tsu.tsi.ts.TransferToSchool ?? "Không có thông tin trường chuyển đến",
                    ProvinceCode = res.tsu.u.ProvinceCode ?? 0,
                    DistrictCode = res.tsu.u.DistrictCode ?? 0,
                    Reason = res.tsu.tsi.ts.Reason ?? "Không có lý do",
                    Attachment = res.tsu.tsi.ts.AttachmentName ?? "Không có tệp đính kèm"
                })
                .FirstOrDefaultAsync();

            if (transferSchool == null)
                return null;

            return new TransferSchoolResponse
            {
                FullName = transferSchool.FullName,
                StudentCode = transferSchool.StudentCode,
                TransferSemester = transferSchool.TransferSemester,
                TransferSchoolDate = transferSchool.TransferSchoolDate,
                TransferToSchool = transferSchool.TransferToSchool,
                Reason = transferSchool.Reason,
                ProvinceCode = transferSchool.ProvinceCode,
                DistrictCode = transferSchool.DistrictCode,
                AttachmentName = transferSchool.Attachment,
                StatusCode = 200
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



