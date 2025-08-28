using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Events;
using TherapistApi.Domain.Interfaces;
using TherapistApi.Domain.Entities;

namespace TherapistApi.Application.Messaging;

// Consumer that handles appointment creation events and updates therapist schedule
public class AppointmentCreatedConsumer : IConsumer<AppointmentCreatedEvent>
{
    private readonly ILogger<AppointmentCreatedConsumer> _logger;
    private readonly ITherapistScheduleRepository _scheduleRepository;

    public AppointmentCreatedConsumer(
        ILogger<AppointmentCreatedConsumer> logger,
        ITherapistScheduleRepository scheduleRepository)
    {
        _logger = logger;
        _scheduleRepository = scheduleRepository;
    }

    public async Task Consume(ConsumeContext<AppointmentCreatedEvent> context)
    {
        var appointmentEvent = context.Message;

        _logger.LogInformation(
            "Received AppointmentCreatedEvent: AppointmentId={AppointmentId}, TherapistId={TherapistId}, DateTime={DateTime}",
            appointmentEvent.AppointmentId,
            appointmentEvent.TherapistId,
            appointmentEvent.DateTime);

        // Update therapist schedule DB
        var scheduleEntry = new TherapistSchedule
        {
            Id = Guid.NewGuid(),
            TherapistId = appointmentEvent.TherapistId,
            AppointmentId = appointmentEvent.AppointmentId,
            AppointmentDateTime = appointmentEvent.DateTime,
            IsBlocked = true,
            Notes = "Appointment scheduled",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _scheduleRepository.AddAsync(scheduleEntry);

        _logger.LogInformation(
            "Therapist schedule updated: TherapistId={TherapistId}, TimeSlot={DateTime}",
            appointmentEvent.TherapistId,
            appointmentEvent.DateTime);
    }
}
