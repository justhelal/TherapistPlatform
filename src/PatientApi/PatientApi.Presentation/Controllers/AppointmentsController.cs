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
            return Ok(result);
        return BadRequest(result);
    }
}
