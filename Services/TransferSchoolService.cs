using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public class TransferSchoolService : ITransferSchoolService
    {
        private readonly TransferSchoolRepo _repository;
        private readonly StudentInfoRepo _studentRepository;
        private readonly UserRepo _userRepository;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;
        private readonly GhnService _ghnService;

        public TransferSchoolService(TransferSchoolRepo repository, IMapper mapper, isc_dbContext context, GhnService ghnService)
        {
            _repository = repository;
            _context = context;
            _ghnService = ghnService;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TransferSchoolResponse>> GetTransferSchoolList()
        {
            var transferSchools = _repository.GetTransferSchoolList();
            var response = _mapper.Map<ICollection<TransferSchoolResponse>>(transferSchools);
            return ApiResponse<ICollection<TransferSchoolResponse>>.Success(response);
        }

  

        /// <summary>
        /// Lấy thông tin chi tiết học sinh chuyển trường theo StudentId.
        /// </summary>
        public async Task<TransferSchoolResponse?> GetTransferSchoolByStudentCode(string studentCode)
        {
            // Tìm studentId từ studentCode
            var student = await _context.Users
                .Where(u => u.Code == studentCode)
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy học sinh với studentCode, trả về null
            if (student == null)
            {
                return null;
            }

            var studentId = student.Id;

            // Tiến hành truy vấn thông tin chuyển trường từ studentId
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

            // Nếu không tìm thấy dữ liệu chuyển trường, trả về null
            if (transferSchool == null)
            {
                return null;
            }

            // Trả về thông tin chuyển trường
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

        public ApiResponse<TransferSchoolResponse> CreateTransferSchool(TransferSchoolRequest request)
        {
            // Tìm StudentId từ StudentCode
            var student = _context.Users.FirstOrDefault(u => u.Code == request.StudentCode);
            if (student == null)
            {
                return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy học sinh với mã StudentCode đã cung cấp.");
            }

            var studentId = student.Id;

            // Kiểm tra xem StudentId đã tồn tại trong bảng TransferSchool chưa
            var isStudentIdExist = _context.TransferSchools.Any(ts => ts.StudentId == studentId);
            if (isStudentIdExist)
            {
                return ApiResponse<TransferSchoolResponse>.Fail("Học sinh này đã có trong danh sách chuyển trường.");
            }

            // Lưu thông tin chuyển trường
            var transferSchool = new TransferSchool
            {
                StudentId = studentId,
                TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate, DateTimeKind.Unspecified),
                TransferToSchool = request.TransferToSchool,
                SchoolAddress = request.SchoolAddress,
                Reason = request.Reason,
                AttachmentName = request.AttachmentName,
                AttachmentPath = request.AttachmentPath,
                SemesterId = request.SemesterId,
                UserId = request.UserId  // Gán userId từ request (lấy từ token)
            };

            var created = _repository.CreateTransferSchool(transferSchool);

            // Lấy thông tin địa phương từ GHN Service
            var (provinceName, districtName, wardName) = _ghnService.GetLocationName(
                request.ProvinceCode ?? 0,
                request.DistrictCode ?? 0,
                "" // Không cần WardCode
            ).Result;

            // Trả về thông tin sau khi lưu
            var transferSchoolRepo = new TransferSchoolResponse
            {
                StudentId = created.StudentId,
                TransferSchoolDate = created.TransferSchoolDate,
                TransferToSchool = created.TransferToSchool,
                Reason = created.Reason,
                AttachmentName = created.AttachmentName,
                AttachmentPath = created.AttachmentPath,
                ProvinceName = provinceName,
                DistrictName = districtName
            };

            return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(transferSchoolRepo));
        }



        public ApiResponse<TransferSchoolResponse> UpdateTransferSchool(string studentCode, TransferSchoolRequest request)
        {
            // Tìm StudentId từ StudentCode**
            var student = _context.Users.FirstOrDefault(u => u.Code == studentCode);
            if (student == null)
            {
                return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy học sinh với mã StudentCode đã cung cấp.");
            }

            var studentId = student.Id;

            //Tìm bản ghi TransferSchool cần cập nhật**
            var existingTransfer = _context.TransferSchools.FirstOrDefault(ts => ts.StudentId == studentId);
            if (existingTransfer == null)
            {
                return ApiResponse<TransferSchoolResponse>.Fail("Không tìm thấy dữ liệu chuyển trường để cập nhật.");
            }

            //Cập nhật thông tin chuyển trường**
            existingTransfer.TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate, DateTimeKind.Unspecified);
            existingTransfer.SchoolAddress = request.SchoolAddress;
            existingTransfer.Reason = request.Reason;
            existingTransfer.AttachmentName = request.AttachmentName;
            existingTransfer.AttachmentPath = request.AttachmentPath;
            existingTransfer.SemesterId = request.SemesterId;
            existingTransfer.UserId = request.UserId;  // Lưu userId từ token

            //*Lưu thay đổi vào DB**
            _context.SaveChanges();

            return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(existingTransfer));
        }

        public ApiResponse<TransferSchoolResponse> GetTransferSchoolByStudentId(int studentId)
        {
            throw new NotImplementedException();
        }
    }

}

