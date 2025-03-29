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

        public TransferSchoolService(TransferSchoolRepo repository, IMapper mapper)
        {
            _repository = repository;

            _mapper = mapper;
        }

        public ApiResponse<ICollection<TransferSchoolResponse>> GetTransferSchoolList()
        {
            var transferSchools = _repository.GetTransferSchoolList();
            var response = _mapper.Map<ICollection<TransferSchoolResponse>>(transferSchools);
            return ApiResponse<ICollection<TransferSchoolResponse>>.Success(response);
        }

        public ApiResponse<TransferSchoolResponse> GetTransferSchoolByStudentId(int studentId)
        {
            var transferSchool = _repository.GetTransferSchoolByStudentId(studentId);
            return transferSchool != null
                ? ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(transferSchool))
                : ApiResponse<TransferSchoolResponse>.NotFound("Không tìm thấy thông tin chuyển trường.");
        }



        public ApiResponse<TransferSchoolResponse> GetTransferSchoolById(int id)
        {
            var transferSchool = _repository.GetTransferSchoolByStudentId(id);
            return transferSchool != null
                ? ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(transferSchool))
                : ApiResponse<TransferSchoolResponse>.NotFound("Không tìm thấy thông tin chuyển trường");
        }

        public ApiResponse<TransferSchoolResponse> CreateTransferSchool(TransferSchoolRequest request)
        {

            var transferSchool = new TransferSchool
            {
                UserId = request.UserId,
                
                StudentId = request.StudentId,
                TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate, DateTimeKind.Unspecified),
               
                SchoolAddress = request.SchoolAddress,
                Reason = request.Reason,
                AttachmentName = request.AttachmentName,
                AttachmentPath = request.AttachmentPath,
                SemesterId = request.SemesterId,
            };

            // Gọi Repository lưu vào DB
            var created = _repository.CreateTransferSchool(transferSchool);

            var transferSchoolRepo = new TransferSchoolResponse
            {

                StudentId = created.StudentId,
                TransferSchoolDate = created.TransferSchoolDate,
            
                Reason = created.Reason,
                AttachmentName = created.AttachmentName,
                AttachmentPath = created.AttachmentPath,
                SemesterId = created.SemesterId,
            };

            // Chuyển Entity -> DTO để trả về
            return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(transferSchoolRepo));
        }




        public ApiResponse<TransferSchoolResponse> UpdateTransferSchool(int studentId, TransferSchoolRequest request)
        {
            // Tìm dữ liệu cần cập nhật
            var existingTransfer = _repository.GetByStudentId(studentId);
            if (existingTransfer == null)
            {
                throw new Exception("Không tìm thấy dữ liệu chuyển trường!");
            }

            // Cập nhật dữ liệu
            existingTransfer.UserId = request.UserId;
            existingTransfer.TransferSchoolDate = DateTime.SpecifyKind(request.TransferSchoolDate, DateTimeKind.Unspecified);
            existingTransfer.SchoolAddress = request.SchoolAddress;
            existingTransfer.Reason = request.Reason;
            existingTransfer.AttachmentName = request.AttachmentName;
            existingTransfer.AttachmentPath = request.AttachmentPath;
            existingTransfer.SemesterId = request.SemesterId;

            // Gọi Repository để cập nhật dữ liệu vào DB
            var updated = _repository.UpdateTransferSchool(existingTransfer);

            // Tạo response để trả về
            var transferSchoolResponse = new TransferSchoolResponse
            {
                StudentId = updated.StudentId,
                TransferSchoolDate = updated.TransferSchoolDate,
                Reason = updated.Reason,
                AttachmentName = updated.AttachmentName,
                AttachmentPath = updated.AttachmentPath,
                SemesterId = updated.SemesterId
            };

            return ApiResponse<TransferSchoolResponse>.Success(transferSchoolResponse);
        }
    }
}

