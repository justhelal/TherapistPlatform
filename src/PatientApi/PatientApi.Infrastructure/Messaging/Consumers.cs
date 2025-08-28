using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;

namespace PatientApi.Infrastructure.Messaging;

// Simple consumer that logs therapist events (optional for this simplified flow)
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
            "Received TherapistCreatedEvent: TherapistId={TherapistId}, Name={FirstName} {LastName}",
            therapistEvent.TherapistId,
            therapistEvent.FirstName,
            therapistEvent.LastName);

        return Task.CompletedTask;
    }
}
