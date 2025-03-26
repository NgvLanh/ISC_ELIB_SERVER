using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchivementController : ControllerBase
    {
        private readonly IAchivementService _service;

        public AchivementController(IAchivementService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAchivements([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string? search = "", [FromQuery] string sortColumn = "Id", [FromQuery] string sortOrder = "asc")
        {
            var response = _service.GetAchivements(page, pageSize, search, sortColumn, sortOrder);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetAchivementById(int id)
        {
            var response = _service.GetAchivementById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateAchivement([FromBody] AchivementRequest AchivementRequest)
        {
            var response = _service.CreateAchivement(AchivementRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAchivement(int id, [FromBody] AchivementRequest AchivementRequest)
        {
            var response = _service.UpdateAchivement(id, AchivementRequest);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAchivement(int id)
        {
            var response = _service.DeleteAchivement(id);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }
    }
}
