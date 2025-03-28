using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/subject-types")]
    public class SubjectTypeController:ControllerBase
    {
        private readonly ISubjectTypeService _service;

        public SubjectTypeController(ISubjectTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetSubjectType([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetSubjectType(page, pageSize, search, sortColumn, sortOrder);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubjectTypeById(long id)
        {
            var response = _service.GetSubjectTypeById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateSubjectType([FromBody] SubjectTypeRequest subjectTypeRequest)
        {
            var response = _service.CreateSubjectType(subjectTypeRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSubjectType(long id, [FromBody] SubjectTypeRequest subjectTypeRequest)
        {

            var response = _service.UpdateSubjectType(id, subjectTypeRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubjectType(long id)
        {
            var response = _service.DeleteSubjectType(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
