using Microsoft.AspNetCore.Mvc;
using TherapistApi.Domain.Interfaces;

namespace TherapistApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ITherapistScheduleRepository _scheduleRepository;

    public ScheduleController(ITherapistScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    [HttpGet("therapist/{therapistId}")]
    public async Task<IActionResult> GetTherapistSchedule(Guid therapistId)
    {
        var schedule = await _scheduleRepository.GetByTherapistIdAsync(therapistId);
        return Ok(schedule);
    }

    [HttpGet("therapist/{therapistId}/availability")]
    public async Task<IActionResult> CheckAvailability(Guid therapistId, [FromQuery] DateTime dateTime)
    {
        var isAvailable = await _scheduleRepository.IsTimeSlotAvailableAsync(therapistId, dateTime);
        return Ok(new { IsAvailable = isAvailable, TherapistId = therapistId, DateTime = dateTime });
    }
}
