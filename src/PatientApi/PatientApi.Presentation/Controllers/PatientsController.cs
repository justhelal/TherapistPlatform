using Microsoft.AspNetCore.Mvc;
using PatientApi.Application.DTOs;
using PatientApi.Application.Interfaces;

namespace PatientApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(Guid id)
    {
        var result = await _patientService.GetPatientByIdAsync(id);
        if (result.Success)
            return Ok(result);
        return NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePatient([FromBody] CreatePatientDto createPatientDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _patientService.CreatePatientAsync(createPatientDto);
        if (result.Success)
            return CreatedAtAction(nameof(GetPatientById), new { id = result.Data?.Id }, result);
        return BadRequest(result);
    }
}
