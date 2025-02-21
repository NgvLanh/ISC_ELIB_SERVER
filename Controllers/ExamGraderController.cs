using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamGraderController : ControllerBase
    {
        private readonly IExamGraderService _service;

        public ExamGraderController(IExamGraderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
                                                [FromQuery] string? sortBy = null, [FromQuery] bool isDescending = false,
                                                [FromQuery] int? examId = null, [FromQuery] int? userId = null)
        {
            var result = await _service.GetAllAsync(page, pageSize, sortBy, isDescending, examId, userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound("ExamGrader không tồn tại");
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamGraderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddAsync(request);
            return Ok("ExamGrader đã được thêm thành công");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExamGraderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.UpdateAsync(id, request);
            return Ok("ExamGrader đã được cập nhật thành công");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok("ExamGrader đã được xóa thành công");
        }
    }
}
