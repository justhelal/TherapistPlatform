namespace TherapistApi.Application.DTOs;

public class TherapistScheduleDto
{
    public Guid Id { get; set; }
    public Guid TherapistId { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public bool IsBlocked { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
