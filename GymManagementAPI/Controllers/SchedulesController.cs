using GymManagementAPI.Models;
using GymManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementAPI.Controllers;

[ApiController]
[Route("api/schedules")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var schedules = await _scheduleService.GetAllAsync();
            return Ok(schedules);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound(new { error = "Schedule not found" });
            }
            return Ok(schedule);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Schedule schedule)
    {
        try
        {
            var createdSchedule = await _scheduleService.CreateAsync(schedule);
            return StatusCode(201, createdSchedule);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Schedule schedule)
    {
        try
        {
            schedule.Id = id;
            var updatedSchedule = await _scheduleService.UpdateAsync(id, schedule);
            if (updatedSchedule == null)
            {
                return NotFound(new { error = "Schedule not found" });
            }
            return Ok(updatedSchedule);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var deleted = await _scheduleService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { error = "Schedule not found" });
            }
            return Ok(new { message = "Schedule deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
