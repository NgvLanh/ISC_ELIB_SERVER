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
        ApiResponse<TransferSchoolResponse> GetTransferSchoolByStudentId(int id);
        ApiResponse<TransferSchoolResponse> CreateTransferSchool(TransferSchoolRequest request);
        ApiResponse<TransferSchoolResponse> UpdateTransferSchool(int id, TransferSchoolRequest request);
        ApiResponse<object> DeleteTransferSchool(int id);
    }
}
