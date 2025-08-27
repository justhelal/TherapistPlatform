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
    private readonly IPatientRepository _patientRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        IPublishEndpoint publishEndpoint)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ApiResponse<AppointmentDto>> CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto)
    {
        try
        {
            // Verify patient exists
            var patientExists = await _patientRepository.ExistsAsync(createAppointmentDto.PatientId);
            if (!patientExists)
                return ApiResponse<AppointmentDto>.ErrorResult("Patient not found");

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

            var createdAppointment = await _appointmentRepository.AddAsync(appointment);

            // Publish AppointmentScheduledEvent
            await _publishEndpoint.Publish(new AppointmentScheduledEvent
            {
                AppointmentId = createdAppointment.Id,
                PatientId = createdAppointment.PatientId,
                TherapistId = createdAppointment.TherapistId,
                AppointmentDateTime = createdAppointment.AppointmentDate,
                Notes = createdAppointment.Notes,
                CreatedAt = createdAppointment.CreatedAt
            });

            return ApiResponse<AppointmentDto>.SuccessResult(MapToDto(createdAppointment), "Appointment scheduled successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<AppointmentDto>.ErrorResult($"Error creating appointment: {ex.Message}");
        }
    }

    public async Task<ApiResponse<AppointmentDto>> UpdateAppointmentAsync(Guid id, CreateAppointmentDto updateAppointmentDto)
    {
        try
        {
            var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
            if (existingAppointment == null)
                return ApiResponse<AppointmentDto>.ErrorResult("Appointment not found");

            // Update properties
            existingAppointment.PatientId = updateAppointmentDto.PatientId;
            existingAppointment.TherapistId = updateAppointmentDto.TherapistId;
            existingAppointment.AppointmentDate = updateAppointmentDto.AppointmentDate;
            existingAppointment.Duration = updateAppointmentDto.Duration;
            existingAppointment.Notes = updateAppointmentDto.Notes;
            existingAppointment.Cost = updateAppointmentDto.Cost;
            existingAppointment.UpdatedAt = DateTime.UtcNow;

            var updatedAppointment = await _appointmentRepository.UpdateAsync(existingAppointment);

            return ApiResponse<AppointmentDto>.SuccessResult(MapToDto(updatedAppointment), "Appointment updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<AppointmentDto>.ErrorResult($"Error updating appointment: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> CancelAppointmentAsync(Guid id, string cancellationReason)
    {
        try
        {
            var existingAppointment = await _appointmentRepository.GetByIdAsync(id);
            if (existingAppointment == null)
                return ApiResponse<bool>.ErrorResult("Appointment not found");

            existingAppointment.Status = AppointmentStatus.Cancelled;
            existingAppointment.SessionNotes = $"Cancelled: {cancellationReason}";
            existingAppointment.UpdatedAt = DateTime.UtcNow;

            await _appointmentRepository.UpdateAsync(existingAppointment);

            // Publish AppointmentCancelledEvent
            await _publishEndpoint.Publish(new AppointmentCancelledEvent
            {
                AppointmentId = existingAppointment.Id,
                PatientId = existingAppointment.PatientId,
                TherapistId = existingAppointment.TherapistId,
                CancellationReason = cancellationReason,
                CancelledAt = existingAppointment.UpdatedAt ?? DateTime.UtcNow
            });

            return ApiResponse<bool>.SuccessResult(true, "Appointment cancelled successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult($"Error cancelling appointment: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<AppointmentDto>>> GetAppointmentsByPatientIdAsync(Guid patientId)
    {
        try
        {
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
            var appointmentDtos = appointments.Select(MapToDto);
            return ApiResponse<IEnumerable<AppointmentDto>>.SuccessResult(appointmentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<AppointmentDto>>.ErrorResult($"Error retrieving appointments: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<AppointmentDto>>> GetUpcomingAppointmentsAsync()
    {
        try
        {
            var appointments = await _appointmentRepository.GetUpcomingAppointmentsAsync();
            var appointmentDtos = appointments.Select(MapToDto);
            return ApiResponse<IEnumerable<AppointmentDto>>.SuccessResult(appointmentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<AppointmentDto>>.ErrorResult($"Error retrieving upcoming appointments: {ex.Message}");
        }
    }

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
            SessionNotes = appointment.SessionNotes,
            Cost = appointment.Cost,
            IsPaid = appointment.IsPaid,
            PatientName = appointment.Patient != null ? $"{appointment.Patient.FirstName} {appointment.Patient.LastName}" : string.Empty
        };
    }
}
