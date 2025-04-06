using System.Collections.Generic;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITransferSchoolService
    {
        ApiResponse<ICollection<TransferSchoolResponse>> GetTransferSchoolList();
        ApiResponse<TransferSchoolResponse> GetTransferSchoolByStudentId(int studentId);
        ApiResponse<TransferSchoolResponse> CreateTransferSchool(TransferSchoolRequest request);
        ApiResponse<TransferSchoolResponse> UpdateTransferSchool(string studentCode, TransferSchoolRequest request);
        ApiResponse<TransferSchoolResponse> GetTransferSchoolByStudentCode(string studentCode);
    }
}
