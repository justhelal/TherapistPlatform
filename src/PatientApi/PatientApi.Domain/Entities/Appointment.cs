using Shared.Common;
using PatientApi.Domain.Enums;

namespace PatientApi.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid PatientId { get; set; }
    public Guid TherapistId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan Duration { get; set; }
    public AppointmentStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string SessionNotes { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public bool IsPaid { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
}
