namespace Shared.Events;

// Event published when a therapist is created
public record TherapistCreatedEvent
{
    public Guid TherapistId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Specialization { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

// Event published when a therapist is updated
public record TherapistUpdatedEvent
{
    public Guid TherapistId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Specialization { get; init; } = string.Empty;
    public DateTime UpdatedAt { get; init; }
}

// Event published when a patient is created
public record PatientCreatedEvent
{
    public Guid PatientId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }
    public DateTime CreatedAt { get; init; }
}

// Event published when an appointment is scheduled
public record AppointmentScheduledEvent
{
    public Guid AppointmentId { get; init; }
    public Guid PatientId { get; init; }
    public Guid TherapistId { get; init; }
    public DateTime AppointmentDateTime { get; init; }
    public string Notes { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}

// Event published when an appointment is cancelled
public record AppointmentCancelledEvent
{
    public Guid AppointmentId { get; init; }
    public Guid PatientId { get; init; }
    public Guid TherapistId { get; init; }
    public string CancellationReason { get; init; } = string.Empty;
    public DateTime CancelledAt { get; init; }
}
