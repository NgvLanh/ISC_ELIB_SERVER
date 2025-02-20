using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.IServices
{
    public interface IChatService
    {
        ApiResponse<ICollection<ChatResponse>> GetChatsBySessionId(long sessionId);
        ApiResponse<ChatResponse> GetChatById(long id);
        ApiResponse<ChatResponse> CreateChat(ChatRequest chatRequest);
        ApiResponse<ChatResponse> UpdateChat(long id, ChatUpdateRequest chatRequest);
        ApiResponse<object> DeleteChat(long id);
    }
}
