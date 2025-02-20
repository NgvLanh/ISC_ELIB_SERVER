using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITransferSchoolService
    {
        ApiResponse<ICollection<TransferSchoolResponse>> GetTransferSchools(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ICollection<TransferSchoolResponse>> GetTransferSchoolsNormal();
        ApiResponse<TransferSchoolResponse> GetTransferSchoolById(long id);
        ApiResponse<TransferSchoolResponse> CreateTransferSchool(TransferSchool_AddRequest TransferSchoolRequest);
        ApiResponse<TransferSchool> UpdateTransferSchool(long id, TransferSchool_UpdateRequest TransferSchool);
        ApiResponse<TransferSchool> DeleteTransferSchool(long id);
    }
}
