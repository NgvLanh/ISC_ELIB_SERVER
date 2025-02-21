using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _service;

        public TestController(ITestService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTestes([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetTestes(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTestById(long id)
        {
            var response = _service.GetTestById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTest([FromBody] TestRequest TestRequest)
        {
            var response = _service.CreateTest(TestRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTest(long id, [FromBody] TestRequest Test)
        {
            var response = _service.UpdateTest(id,Test);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTest(long id)
        {
            var response = _service.DeleteTest(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

    }
}
