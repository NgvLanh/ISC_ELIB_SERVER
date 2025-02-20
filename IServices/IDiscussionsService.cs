using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.IServices
{
    public interface IDiscussionsService
    {
        ApiResponse<ICollection<DiscussionResponse>> GetDiscussions(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<DiscussionResponse> GetDiscussionById(long id);
        ApiResponse<DiscussionResponse> CreateDiscussion(DiscussionRequest request);
        ApiResponse<DiscussionResponse> UpdateDiscussion(long id, DiscussionRequest request);
        ApiResponse<bool> DeleteDiscussion(long id);
    }
}
