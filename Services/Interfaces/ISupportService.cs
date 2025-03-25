using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ISupportService
    {
        ApiResponse<ICollection<SupportResponse>> GetSupports(int? page, int? pageSize, string? sortColumn, string? sortOrder);
        ApiResponse<SupportResponse> GetSupportById(long id);
        ApiResponse<SupportResponse> CreateSupport(SupportRequest SupportRequest);
        ApiResponse<Support> UpdateSupport(Support Support);
        ApiResponse<Support> DeleteSupport(long id);
    }
}
