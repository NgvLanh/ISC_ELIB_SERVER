using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Responses.ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;
using static ISC_ELIB_SERVER.Services.ScoreTypeService;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/score-type")]
    public class ScoreTypeController : ControllerBase
    {
        private readonly IScoreTypeService _service;

        public ScoreTypeController(IScoreTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetScoreType([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetScoreTypes(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }


        [HttpGet("{id}")]
        public IActionResult GetScoreTypeById(long id)
        {
            var response = _service.GetScoreTypeById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateScoreType([FromBody] ScoreTypeRequest scoreTypeRequest)
        {
            var response = _service.CreateScoreType(scoreTypeRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateScoreType(long id, [FromBody] ScoreTypeRequest scoreTypeRequest)
        {

            var response = _service.UpdateScoreType(id, scoreTypeRequest);

            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteScoreType(long id)
        {
            var response = _service.DeleteScoreType(id);

            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpGet("by-name")]
        public IActionResult GetScoreTypeByName([FromQuery] string name)
        {
            var response = _service.GetScoreTypeByName(name);
            return StatusCode(response.Code, response);
        }

    }
}
