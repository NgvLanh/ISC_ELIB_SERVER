using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/reserves")]
    public class ReserveController : ControllerBase
    {
        private readonly IReserveService _service;

        public ReserveController(IReserveService service)
        {
            _service = service;
        }

        // GET: api/reserves/active
        [HttpGet("active")]
        public IActionResult GetActiveReserves([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetActiveReserves(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        // GET: api/reserves/{id}
        [HttpGet("{id}")]
        public IActionResult GetReserveById(long id)
        {
            var response = _service.GetReserveById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        // GET: api/reserves/student/{studentId}
        [HttpGet("student/{studentId}")]
        public IActionResult GetReserveByStudentId(int studentId)
        {
            var response = _service.GetReserveByStudentId(studentId);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateReserve([FromBody] ReserveRequest reserveRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = _service.CreateReserve(reserveRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReserve(long id, [FromBody] Reserve reserve)
        {

            var response = _service.UpdateReserve(reserve);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReserve(long id)
        {
            var response = _service.DeleteReserve(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
