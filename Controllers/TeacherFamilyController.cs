using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/teacher-family")]
    [ApiController]
    public class TeacherFamilyController : ControllerBase
    {
        private readonly ITeacherFamilyService _service;

        public TeacherFamilyController(ITeacherFamilyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherFamilyRequest request)
        {
            await _service.AddAsync(request);
            return CreatedAtAction(nameof(GetAll), new { });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] TeacherFamilyRequest request)
        {
            await _service.UpdateAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
