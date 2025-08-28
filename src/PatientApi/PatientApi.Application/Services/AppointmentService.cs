using PatientApi.Application.DTOs;
using PatientApi.Application.Interfaces;
using PatientApi.Domain.Entities;
using PatientApi.Domain.Enums;
using PatientApi.Domain.Interfaces;
using Shared.Common;
using MassTransit;
using Shared.Events;

namespace PatientApi.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPublishEndpoint publishEndpoint)
    {
        _appointmentRepository = appointmentRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ApiResponse<AppointmentDto>> CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto)
    {
        try
        {
            // Validate patient and therapist IDs exist (simple validation)
            if (createAppointmentDto.PatientId == Guid.Empty)
                return ApiResponse<AppointmentDto>.ErrorResult("Invalid patient ID");
            
            if (createAppointmentDto.TherapistId == Guid.Empty)
                return ApiResponse<AppointmentDto>.ErrorResult("Invalid therapist ID");

            // Create appointment entity
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                PatientId = createAppointmentDto.PatientId,
                TherapistId = createAppointmentDto.TherapistId,
                AppointmentDate = createAppointmentDto.AppointmentDate,
                Duration = createAppointmentDto.Duration,
                Status = AppointmentStatus.Scheduled,
                Notes = createAppointmentDto.Notes,
                Cost = createAppointmentDto.Cost,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save appointment in Patient DB
            var createdAppointment = await _appointmentRepository.AddAsync(appointment);

            // Publish AppointmentCreated event to RabbitMQ
            await _publishEndpoint.Publish(new AppointmentCreatedEvent
            {
                AppointmentId = createdAppointment.Id,
                PatientId = createdAppointment.PatientId,
                TherapistId = createdAppointment.TherapistId,
                DateTime = createdAppointment.AppointmentDate
            });

            return ApiResponse<AppointmentDto>.SuccessResult(MapToDto(createdAppointment), "Appointment created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<AppointmentDto>.ErrorResult($"Error creating appointment: {ex.Message}");
        }
    }

    // Keep only the mapping method for the simplified flow
    private static AppointmentDto MapToDto(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            TherapistId = appointment.TherapistId,
            AppointmentDate = appointment.AppointmentDate,
            Duration = appointment.Duration,
            Status = appointment.Status,
            Notes = appointment.Notes,
            Cost = appointment.Cost
        };
    }
}
