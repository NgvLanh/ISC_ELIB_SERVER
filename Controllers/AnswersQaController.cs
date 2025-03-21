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

        [HttpPost]
        [Consumes("multipart/form-data")] // 📌 Để nhận file ảnh từ FE
        public async Task<IActionResult> CreateAnswer([FromForm] AnswersQaRequest answerRequest)
        {
            var response = await _service.CreateAnswer(answerRequest);
            return Ok(response);
        }


        
        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(long id)
        {
            var response = _service.DeleteAnswer(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }


        [HttpGet("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult GetAnswerById(long id)
        {
            var response = _service.GetAnswerById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPut("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult UpdateAnswer(long id, [FromBody] AnswersQaRequest answerRequest)
        {
            var response = _service.UpdateAnswer(id, answerRequest);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }



    }
}
