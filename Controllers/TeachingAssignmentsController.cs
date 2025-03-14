using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/teaching-assignments")]
    public class TeachingAssignmentsController : ControllerBase
    {
        private readonly ITeachingAssignmentsService _service;

        public TeachingAssignmentsController(ITeachingAssignmentsService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTeachingAssignments(
            [FromQuery] int? page = 1,
            [FromQuery] int? pageSize = 10,
            [FromQuery] string? sortColumn = "Id",
            [FromQuery] string? sortOrder = "asc")
        {
            var response = _service.GetTeachingAssignments(page, pageSize, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTeachingAssignmentById(int id)
        {
            var response = _service.GetTeachingAssignmentById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTeachingAssignment([FromBody] TeachingAssignmentsRequest request)
        {
            var response = _service.CreateTeachingAssignment(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeachingAssignment(int id, [FromBody] TeachingAssignmentsRequest request)
        {
            var response = _service.UpdateTeachingAssignment(id, request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeachingAssignment(int id)
        {
            var response = _service.DeleteTeachingAssignment(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
