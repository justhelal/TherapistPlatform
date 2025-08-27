using PatientApi.Application.DTOs;
using PatientApi.Domain.Entities;
using PatientApi.Domain.Interfaces;
using Shared.Common;
using MassTransit;
using Shared.Events;

namespace PatientApi.Application.Interfaces;

public interface IAppointmentService
{
    Task<ApiResponse<AppointmentDto>> CreateAppointmentAsync(CreateAppointmentDto createAppointmentDto);
    Task<ApiResponse<AppointmentDto>> UpdateAppointmentAsync(Guid id, CreateAppointmentDto updateAppointmentDto);
    Task<ApiResponse<bool>> CancelAppointmentAsync(Guid id, string cancellationReason);
    Task<ApiResponse<IEnumerable<AppointmentDto>>> GetAppointmentsByPatientIdAsync(Guid patientId);
    Task<ApiResponse<IEnumerable<AppointmentDto>>> GetUpcomingAppointmentsAsync();
}
