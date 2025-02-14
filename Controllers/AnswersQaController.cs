using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/answers-qa")]
    public class AnswersQaController : ControllerBase
    {
        private readonly IAnswersQaService _service;

        public AnswersQaController(IAnswersQaService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAnswers([FromQuery] long? questionId)
        {
            var response = _service.GetAnswers(questionId);
            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetAnswerById(long id)
        {
            var response = _service.GetAnswerById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateAnswer([FromBody] AnswersQaRequest answerRequest)
        {
            var response = _service.CreateAnswer(answerRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAnswer(long id, [FromBody] AnswersQaRequest answerRequest)
        {
            var response = _service.UpdateAnswer(id, answerRequest);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(long id)
        {
            var response = _service.DeleteAnswer(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
