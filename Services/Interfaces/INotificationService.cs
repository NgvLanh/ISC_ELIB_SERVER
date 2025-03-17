using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface INotificationService
    {
        ApiResponse<ICollection<NotificationResponse>> GetNotifications(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<NotificationResponse> GetNotificationById(long id);
        ApiResponse<NotificationResponse> CreateNotification(NotificationRequest notificationRequest);
        ApiResponse<NotificationResponse> UpdateNotification(long id, NotificationRequest notificationRequest);
        ApiResponse<Notification> DeleteNotification(long id);
    }
}
