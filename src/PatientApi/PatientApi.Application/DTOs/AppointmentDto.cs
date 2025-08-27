using PatientApi.Domain.Enums;

namespace PatientApi.Application.DTOs;

public class AppointmentDto
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan Duration { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string SessionNotes { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public bool IsPaid { get; set; }
    public string PatientName { get; set; } = string.Empty;
}
