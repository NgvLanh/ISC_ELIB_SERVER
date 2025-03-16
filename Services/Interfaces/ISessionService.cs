using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
//using System.collections.Generusing ISC_ELIB_SERVER.Services.Interfaces;
namespace ISC_ELIB_SERVER.Services.Interfaces;

public interface ISessionService
{
    ApiResponse<ICollection<SessionResponse>> GetSessions(int page, int pageSize, string search, string sortColumn, string sortOrder);
    ApiResponse<SessionResponse> GetSessionById(int id);
    ApiResponse<SessionResponse> CreateSession(SessionRequest request);
    ApiResponse<SessionResponse> UpdateSession(int id, SessionRequest request);
    ApiResponse<string> DeleteSession(int id);
}



