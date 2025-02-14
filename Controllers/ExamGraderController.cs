using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/exam-graders")]
    public class ExamGraderController : ControllerBase
    {
        private readonly ExamGraderService _service;

        public ExamGraderController(ExamGraderService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _service.GetAll();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var response = _service.GetById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ExamGraderRequest request)
        {
            var response = _service.Create(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] ExamGraderRequest request)
        {
            var response = _service.Update(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var response = _service.Delete(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
