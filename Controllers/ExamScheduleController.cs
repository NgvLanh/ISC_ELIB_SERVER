using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ExamScheduleController : ControllerBase
{
    private readonly IExamScheduleService _examScheduleService;

    public ExamScheduleController(IExamScheduleService examScheduleService)
    {
        _examScheduleService = examScheduleService;
    }

    [HttpGet]
    public IActionResult GetAllExamSchedules()
    {
        var schedules = _examScheduleService.GetExamSchedules();
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public IActionResult GetExamScheduleById(int id)
    {
        var schedule = _examScheduleService.GetExamScheduleById(id);
        if (schedule == null)
        {
            return NotFound();
        }
        return Ok(schedule);
    }

    [HttpPost]
    public IActionResult CreateExamSchedule([FromBody] ExamScheduleRequest request)
    {
        _examScheduleService.CreateExamSchedule(request);
        return Ok(new { message = "Exam schedule created successfully." });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateExamSchedule(int id, [FromBody] ExamScheduleRequest request)
    {
        var existingSchedule = _examScheduleService.GetExamScheduleById(id);
        if (existingSchedule == null)
        {
            return NotFound();
        }

        _examScheduleService.UpdateExamSchedule(id, request);
        return Ok(new { message = "Exam schedule updated successfully." });
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteExamSchedule(int id)
    {
        var existingSchedule = _examScheduleService.GetExamScheduleById(id);
        if (existingSchedule == null)
        {
            return NotFound();
        }

        _examScheduleService.DeleteExamSchedule(id);
        return Ok(new { message = "Exam schedule deleted successfully." });
    }
}
