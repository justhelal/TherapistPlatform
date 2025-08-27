using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace PatientApi.Infrastructure.Messaging;

// Consumer that handles therapist creation events in the Patient API
public class TherapistCreatedConsumer : IConsumer<TherapistCreatedEvent>
{
    private readonly ILogger<TherapistCreatedConsumer> _logger;

    public TherapistCreatedConsumer(ILogger<TherapistCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TherapistCreatedEvent> context)
    {
        var therapistEvent = context.Message;

        _logger.LogInformation(
            "Received TherapistCreatedEvent: TherapistId={TherapistId}, Name={FirstName} {LastName}, Specialization={Specialization}",
            therapistEvent.TherapistId,
            therapistEvent.FirstName,
            therapistEvent.LastName,
            therapistEvent.Specialization);

        // Here you could:
        // 1. Store therapist information for reference during appointment booking
        // 2. Update available therapists cache
        // 3. Send notifications to patients
        // 4. Update recommendation system, etc.

        return Task.CompletedTask;
    }
}

// Consumer that handles therapist update events in the Patient API
public class TherapistUpdatedConsumer : IConsumer<TherapistUpdatedEvent>
{
    private readonly ILogger<TherapistUpdatedConsumer> _logger;

    public TherapistUpdatedConsumer(ILogger<TherapistUpdatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TherapistUpdatedEvent> context)
    {
        var therapistEvent = context.Message;

        _logger.LogInformation(
            "Received TherapistUpdatedEvent: TherapistId={TherapistId}, Name={FirstName} {LastName}, Specialization={Specialization}",
            therapistEvent.TherapistId,
            therapistEvent.FirstName,
            therapistEvent.LastName,
            therapistEvent.Specialization);

        // Here you could:
        // 1. Update cached therapist information
        // 2. Notify patients of changes
        // 3. Update appointment validity
        // 4. Refresh recommendation system, etc.

        return Task.CompletedTask;
    }
}
