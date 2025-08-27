using Shared.Common;

namespace TherapistApi.Domain.Entities;

public class Availability : BaseEntity
{
    public Guid TherapistId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsRecurring { get; set; }
    public DateTime? SpecificDate { get; set; }

    // Navigation property
    public Therapist Therapist { get; set; } = null!;
}
