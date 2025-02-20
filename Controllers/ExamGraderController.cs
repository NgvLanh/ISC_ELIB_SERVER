using Microsoft.AspNetCore.Mvc;

namespace ISC_ELIB_SERVER.Controllers
{
    [Route("api/exam-graders")]
    [ApiController]
    public class ExamGraderController : ControllerBase
    {
        private readonly IExamGraderService _examGraderService;

        public ExamGraderController(IExamGraderService examGraderService)
        {
            _examGraderService = examGraderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExamGraders()
        {
            var examGraders = await _examGraderService.GetAllExamGradersAsync();
            return Ok(examGraders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExamGraderById(int id)
        {
            var examGrader = await _examGraderService.GetExamGraderByIdAsync(id);
            if (examGrader == null) return NotFound();
            return Ok(examGrader);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExamGrader([FromBody] ExamGraderRequest request)
        {
            var examGrader = await _examGraderService.CreateExamGraderAsync(request);
            return CreatedAtAction(nameof(GetExamGraderById), new { id = examGrader.Id }, examGrader);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExamGrader(int id, [FromBody] ExamGraderRequest request)
        {
            var updatedExamGrader = await _examGraderService.UpdateExamGraderAsync(id, request);
            if (updatedExamGrader == null) return NotFound();
            return Ok(updatedExamGrader);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExamGrader(int id)
        {
            var result = await _examGraderService.DeleteExamGraderAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
