using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public interface INotificationService
    {
        ApiResponse<ICollection<NotificationResponse>> GetNotifications(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<NotificationResponse> GetNotificationById(long id);
        ApiResponse<NotificationResponse> CreateNotification(NotificationRequest notificationRequest);
        ApiResponse<Notification> UpdateNotification(Notification notification);
        ApiResponse<Notification> DeleteNotification(long id);
    }

    public class NotificationService : INotificationService
    {
        private readonly NotificationRepo _repository;
        private readonly IMapper _mapper;

        public NotificationService(NotificationRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<NotificationResponse>> GetNotifications(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetNotifications().AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(n => n.Title.Contains(search) || n.Content.Contains(search));
            }

            query = sortColumn switch
            {
                "CreateAt" => sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) ? query.OrderByDescending(n => n.CreateAt) : query.OrderBy(n => n.CreateAt),
                "Id" => sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) ? query.OrderByDescending(n => n.Id) : query.OrderBy(n => n.Id),
                _ => query.OrderBy(n => n.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<NotificationResponse>>(result);
            return result.Any() ? ApiResponse<ICollection<NotificationResponse>>.Success(response) : ApiResponse<ICollection<NotificationResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<NotificationResponse> GetNotificationById(long id)
        {
            var notification = _repository.GetNotificationById(id);
            return notification != null ? ApiResponse<NotificationResponse>.Success(_mapper.Map<NotificationResponse>(notification)) : ApiResponse<NotificationResponse>.NotFound($"Không tìm thấy thông báo #{id}");
        }

        public ApiResponse<NotificationResponse> CreateNotification(NotificationRequest notificationRequest)
        {
            var notification = _mapper.Map<Notification>(notificationRequest);
            var created = _repository.CreateNotification(notification);
            return ApiResponse<NotificationResponse>.Success(_mapper.Map<NotificationResponse>(created));
        }

        public ApiResponse<Notification> UpdateNotification(Notification notification)
        {
            var updated = _repository.UpdateNotification(notification);
            return updated != null ? ApiResponse<Notification>.Success(updated) : ApiResponse<Notification>.NotFound("Không tìm thấy thông báo để cập nhật");
        }

        public ApiResponse<Notification> DeleteNotification(long id)
        {
            var success = _repository.DeleteNotification(id);
            return success ? ApiResponse<Notification>.Success() : ApiResponse<Notification>.NotFound("Không tìm thấy thông báo để xóa");
        }
    }
}
