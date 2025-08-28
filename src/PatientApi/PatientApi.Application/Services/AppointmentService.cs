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
            // Validate patient and therapist IDs exist (simple validation)
            if (createAppointmentDto.PatientId == Guid.Empty)
                return ApiResponse<AppointmentDto>.ErrorResult("Invalid patient ID");
            
            if (createAppointmentDto.TherapistId == Guid.Empty)
                return ApiResponse<AppointmentDto>.ErrorResult("Invalid therapist ID");

            // Verify patient exists and get patient details
            var patient = await _patientRepository.GetByIdAsync(createAppointmentDto.PatientId);
            if (patient == null)
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

            return ApiResponse<AppointmentDto>.SuccessResult(MapToDto(createdAppointment, patient), "Appointment created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<AppointmentDto>.ErrorResult($"Error creating appointment: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<AppointmentDto>>> GetAppointmentsByPatientIdAsync(Guid patientId)
    {
        try
        {
            // Verify patient exists
            var patient = await _patientRepository.GetByIdAsync(patientId);
            if (patient == null)
                return ApiResponse<IEnumerable<AppointmentDto>>.ErrorResult("Patient not found");

            // Get appointments for the patient
            var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);
            var appointmentDtos = appointments.Select(a => MapToDto(a, patient));
            
            return ApiResponse<IEnumerable<AppointmentDto>>.SuccessResult(appointmentDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<AppointmentDto>>.ErrorResult($"Error retrieving appointments: {ex.Message}");
        }
    }

    // Keep only the mapping method for the simplified flow
    private static AppointmentDto MapToDto(Appointment appointment, Patient? patient = null)
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
            Cost = appointment.Cost,
            PatientName = patient?.FullName ?? string.Empty
        };
    }
}
