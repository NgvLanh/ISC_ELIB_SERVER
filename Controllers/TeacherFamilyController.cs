using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [ApiController]
    [Route("api/teacher-families")]
    public class TeacherFamilyController : ControllerBase
    {
        private readonly ITeacherFamilyService _service;

        public TeacherFamilyController(ITeacherFamilyService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetTeacherFamilies()
        {
            var response = _service.GetTeacherFamilies();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetTeacherFamilyById(long id)
        {
            var response = _service.GetTeacherFamilyById(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public IActionResult CreateTeacherFamily([FromBody] TeacherFamilyRequest request)
        {
            var response = _service.CreateTeacherFamily(request);
            return response.Code == 0 ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeacherFamily(long id, [FromBody] TeacherFamilyRequest request)
        {
            var response = _service.UpdateTeacherFamily(id, request);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeacherFamily(long id)
        {
            var response = _service.DeleteTeacherFamily(id);
            return response.Code == 0 ? Ok(response) : NotFound(response);
        }
    }
}
