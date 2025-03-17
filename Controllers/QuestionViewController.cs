using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/question-view")]
    public class QuestionViewController : ControllerBase
    {
        private readonly IQuestionViewService _service;

        public QuestionViewController(IQuestionViewService service)
        {
            _service = service;
        }

        // API lấy số lượt xem của câu hỏi
        [HttpGet("{questionId}")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult GetViewCount(int questionId)
        {
            var response = _service.GetViewCount(questionId);
            return Ok(response);
        }

        // API thêm lượt xem
        [HttpPost]
        public IActionResult AddView([FromBody] QuestionViewRequest request)
        {
            _service.AddView(request);
            return Ok(new { message = "Lượt xem đã được cập nhật." });
        }
    }
}
