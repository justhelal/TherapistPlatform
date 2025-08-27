using Microsoft.AspNetCore.Mvc;
using PatientApi.Application.DTOs;
using PatientApi.Application.Interfaces;

namespace PatientApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDto createAppointmentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appointmentService.CreateAppointmentAsync(createAppointmentDto);
        if (result.Success)
            return CreatedAtAction(nameof(GetAppointmentsByPatient), new { patientId = result.Data?.PatientId }, result);
        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] CreateAppointmentDto updateAppointmentDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _appointmentService.UpdateAppointmentAsync(id, updateAppointmentDto);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelAppointment(Guid id, [FromBody] CancelAppointmentDto cancelDto)
    {
        var result = await _appointmentService.CancelAppointmentAsync(id, cancelDto.CancellationReason);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetAppointmentsByPatient(Guid patientId)
    {
        var result = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcomingAppointments()
    {
        var result = await _appointmentService.GetUpcomingAppointmentsAsync();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}

public class CancelAppointmentDto
{
    public string CancellationReason { get; set; } = string.Empty;
}
