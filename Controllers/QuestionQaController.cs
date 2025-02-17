using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/question-qa")]
    public class QuestionQaController : ControllerBase
    {
        private readonly IQuestionQaService _service;

        public QuestionQaController(IQuestionQaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetQuestions([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetQuestions(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestionById(long id)
        {
            var response = _service.GetQuestionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateQuestion([FromBody] QuestionQaRequest questionRequest)
        {
            var response = _service.CreateQuestion(questionRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateQuestion(long id, [FromBody] QuestionQaRequest question)
        {
            var response = _service.UpdateQuestion(id, question);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(long id)
        {
            var response = _service.DeleteQuestion(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
