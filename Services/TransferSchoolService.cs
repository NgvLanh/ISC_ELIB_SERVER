using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            // Chuyển DTO -> Entity
            var transferSchool = _mapper.Map<TransferSchool>(request);

            // Gọi Repository lưu vào DB
            var created = _repository.CreateTransferSchool(transferSchool);

            // Chuyển Entity -> DTO để trả về
            return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(created));
        }



        public ApiResponse<object> DeleteTransferSchool(int id)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<TransferSchoolResponse> UpdateTransferSchool(int id, TransferSchoolRequest request)
        {
            var transferSchool = _mapper.Map<TransferSchoolRequest>(request);
            var updated = _repository.UpdateTransferSchool(transferSchool);

            return ApiResponse<TransferSchoolResponse>.Success(_mapper.Map<TransferSchoolResponse>(updated));
        }
    }
}

