using ISC_ELIB_SERVER.Dto.Request;
using ISC_ELIB_SERVER.Requests;
using ISC_ELIB_SERVER.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleClassController : ControllerBase
    {
        private readonly IExamScheduleClassService _service;

        public ExamScheduleClassController(IExamScheduleClassService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExamScheduleClassRequest request)
        {
            return Ok(await _service.AddAsync(request));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExamScheduleClassRequest request)
        {
            var result = await _service.UpdateAsync(id, request);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeleteAsync(id) ? Ok() : NotFound();
        }
    }
}
