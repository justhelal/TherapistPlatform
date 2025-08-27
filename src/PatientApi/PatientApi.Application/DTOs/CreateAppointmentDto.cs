using System.ComponentModel.DataAnnotations;

namespace PatientApi.Application.DTOs;

public class CreateAppointmentDto
{
    [Required]
    public Guid PatientId { get; set; }
    
    [Required]
    public Guid TherapistId { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    public TimeSpan Duration { get; set; } = TimeSpan.FromMinutes(50); // Default 50-minute session
    
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;
    
    [Range(0, double.MaxValue, ErrorMessage = "Cost must be a positive value")]
    public decimal Cost { get; set; }
}
