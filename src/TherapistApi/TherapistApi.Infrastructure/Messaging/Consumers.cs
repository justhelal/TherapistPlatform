using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace TherapistApi.Infrastructure.Messaging;

// Consumer that handles patient creation events in the Therapist API
public class PatientCreatedConsumer : IConsumer<PatientCreatedEvent>
{
    private readonly ILogger<PatientCreatedConsumer> _logger;

    public PatientCreatedConsumer(ILogger<PatientCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<PatientCreatedEvent> context)
    {
        var patientEvent = context.Message;

        _logger.LogInformation(
            "Received PatientCreatedEvent: PatientId={PatientId}, Name={FirstName} {LastName}, Email={Email}",
            patientEvent.PatientId,
            patientEvent.FirstName,
            patientEvent.LastName,
            patientEvent.Email);

        // Here you could:
        // 1. Store patient information for reference
        // 2. Update therapist availability
        // 3. Send notifications
        // 4. Update analytics, etc.

        return Task.CompletedTask;
    }
}

// Consumer that handles appointment events in the Therapist API
public class AppointmentScheduledConsumer : IConsumer<AppointmentScheduledEvent>
{
    private readonly ILogger<AppointmentScheduledConsumer> _logger;

    public AppointmentScheduledConsumer(ILogger<AppointmentScheduledConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<AppointmentScheduledEvent> context)
    {
        var appointmentEvent = context.Message;

        _logger.LogInformation(
            "Received AppointmentScheduledEvent: AppointmentId={AppointmentId}, TherapistId={TherapistId}, PatientId={PatientId}, DateTime={AppointmentDateTime}",
            appointmentEvent.AppointmentId,
            appointmentEvent.TherapistId,
            appointmentEvent.PatientId,
            appointmentEvent.AppointmentDateTime);

        // Here you could:
        // 1. Update therapist's schedule/availability
        // 2. Send notifications to the therapist
        // 3. Update workload analytics
        // 4. Block the time slot, etc.

        return Task.CompletedTask;
    }
}
