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
        public IActionResult GetQuestions(
            [FromQuery] int iduser,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "",
            [FromQuery] string sortColumn = "Id",
            [FromQuery] string sortOrder = "asc",
            [FromQuery] int? classId = null,
            [FromQuery] int? subjectId = null)
        {
            if (!classId.HasValue || !subjectId.HasValue)
            {
                return BadRequest("Thiếu ClassId hoặc SubjectId");
            }

            var response = _service.GetQuestions(iduser, page, pageSize, search, sortColumn, sortOrder, classId, subjectId);
            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> CreateQuestion([FromForm] QuestionQaRequest questionRequest, [FromForm] List<IFormFile> files)
        {
            if (questionRequest == null)
            {
                return BadRequest(ApiResponse<QuestionQaResponse>.BadRequest("Dữ liệu không hợp lệ"));
            }

            var response = await _service.CreateQuestion(questionRequest, files);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteQuestion(long id)
        {
            var response = _service.DeleteQuestion(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }


        

       [HttpGet("answered")]
        public IActionResult GetAnsweredQuestions(
            [FromQuery] int iduser,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? classId = null,
            [FromQuery] int? subjectId = null)
        {
            var response = _service.GetAnsweredQuestions(iduser, page, pageSize, classId, subjectId);
            return Ok(response);
        }

            [HttpGet("search")]
            public IActionResult SearchQuestionsByUserName(
                [FromQuery] int iduser,
                [FromQuery] string userName,
                [FromQuery] bool onlyAnswered = false,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] int? classId = null,
                [FromQuery] int? subjectId = null)
            {
                var response = _service.SearchQuestionsByUserName(iduser, userName, onlyAnswered, page, pageSize, classId, subjectId);
                return Ok(response);
            }


        [HttpGet("recent")]
        public IActionResult GetRecentQuestions(
            [FromQuery] int iduser,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? classId = null,
            [FromQuery] int? subjectId = null)
        {
            var response = _service.GetRecentQuestions(iduser, page, pageSize, classId, subjectId);
            return Ok(response);
        }



        [HttpPut("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult UpdateQuestion(long id, [FromBody] QuestionQaRequest question)
        {
            var response = _service.UpdateQuestion(id, question);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{idqs}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetQuestionById(long id)
        {
            var response = _service.GetQuestionById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{idus}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetQuestionById(int id, [FromQuery] int userId)
        {
            var response = _service.GetQuestionByIdForUser(id, userId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
    
}
