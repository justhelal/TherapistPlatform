using Microsoft.AspNetCore.Mvc;
using TherapistApi.Application.Interfaces;

namespace TherapistApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ITherapistScheduleService _scheduleService;

    public ScheduleController(ITherapistScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet("therapist/{therapistId}")]
    public async Task<IActionResult> GetTherapistSchedule(Guid therapistId)
    {
        var schedule = await _scheduleService.GetTherapistScheduleAsync(therapistId);
        return Ok(schedule);
    }

    [HttpGet("therapist/{therapistId}/blocked")]
    public async Task<IActionResult> IsTimeSlotBlocked(Guid therapistId, [FromQuery] DateTime dateTime)
    {
        var isBlocked = await _scheduleService.IsTimeSlotBlockedAsync(therapistId, dateTime);
        return Ok(new { IsBlocked = isBlocked, TherapistId = therapistId, DateTime = dateTime });
    }
}
