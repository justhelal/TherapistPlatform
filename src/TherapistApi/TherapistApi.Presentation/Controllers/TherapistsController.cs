using Microsoft.AspNetCore.Mvc;
using TherapistApi.Application.Interfaces;
using TherapistApi.Domain.DTOs;

namespace TherapistApi.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TherapistsController : ControllerBase
{
    private readonly ITherapistService _therapistService;

    public TherapistsController(ITherapistService therapistService)
    {
        _therapistService = therapistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTherapists()
    {
        var result = await _therapistService.GetAllTherapistsAsync();
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTherapistById(Guid id)
    {
        var result = await _therapistService.GetTherapistByIdAsync(id);
        if (result.Success)
            return Ok(result);
        return NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTherapist([FromBody] CreateTherapistDto createTherapistDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _therapistService.CreateTherapistAsync(createTherapistDto);
        if (result.Success)
            return CreatedAtAction(nameof(GetTherapistById), new { id = result.Data?.Id }, result);
        return BadRequest(result);
    }
}
