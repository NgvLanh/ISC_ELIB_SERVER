using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationController(INotificationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetNotifications([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetNotifications(page, pageSize, search, sortColumn, sortOrder);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetNotificationById(long id)
        {
            var response = _service.GetNotificationById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateNotification([FromBody] NotificationRequest notificationRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = _service.CreateNotification(notificationRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
//        POST
//     {
//    "title": "Thông báo 8",
//    "content": "Mật khẩu thay đổi thành công",
//    "type": "Bảo mật",
//    "createAt": "2025-02-15T10:00:00",
//    "senderId": 11,
//    "userId": 12
//    }

        [HttpPut("{id}")]
        public IActionResult UpdateNotification(long id, [FromBody] Notification notification)
        {
            var response = _service.UpdateNotification(notification);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
//        PUT
//    {
//    "id": 37,
//    "title": "Thông báo 222",
//    "content": "Mật khẩu thành công",
//    "type": "Bảo mật",
//    "createAt": "2025-02-15T10:00:00",
//    "senderId": 11,
//    "userId": 12
//    }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(long id)
        {
            var response = _service.DeleteNotification(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
