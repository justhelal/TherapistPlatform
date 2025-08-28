using Shared.Common;

namespace TherapistApi.Domain.Entities;

public class TherapistSchedule : BaseEntity
{
    public Guid TherapistId { get; set; }
    public Guid AppointmentId { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public bool IsBlocked { get; set; } = true;
    public string Notes { get; set; } = string.Empty;

    // Navigation property
    public Therapist Therapist { get; set; } = null!;
}
