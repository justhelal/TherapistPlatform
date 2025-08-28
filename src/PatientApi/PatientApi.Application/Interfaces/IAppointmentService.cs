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
}
