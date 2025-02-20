using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.IServices
{
    public interface IDiscussionImageService
    {
        ApiResponse<ICollection<DiscussionImageResponse>> GetDiscussionImagesByDiscussionId(long discussionId);
        ApiResponse<DiscussionImageResponse> GetDiscussionImageById(long id);
        ApiResponse<DiscussionImageResponse> CreateDiscussionImage(DiscussionImageRequest request);
        ApiResponse<DiscussionImageResponse> UpdateDiscussionImage(long id, DiscussionImageRequest request);
        ApiResponse<bool> DeleteDiscussionImage(long id);
    }
}
