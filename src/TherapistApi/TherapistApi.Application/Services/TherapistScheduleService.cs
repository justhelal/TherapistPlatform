using TherapistApi.Application.DTOs;
using TherapistApi.Application.Interfaces;
using TherapistApi.Domain.Interfaces;

namespace TherapistApi.Application.Services;

public class TherapistScheduleService : ITherapistScheduleService
{
    private readonly ITherapistScheduleRepository _scheduleRepository;

    public TherapistScheduleService(ITherapistScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task<IEnumerable<TherapistScheduleDto>> GetTherapistScheduleAsync(Guid therapistId)
    {
        var schedules = await _scheduleRepository.GetByTherapistIdAsync(therapistId);
        return schedules.Select(s => new TherapistScheduleDto
        {
            Id = s.Id,
            TherapistId = s.TherapistId,
            AppointmentId = s.AppointmentId,
            AppointmentDateTime = s.AppointmentDateTime,
            IsBlocked = s.IsBlocked,
            Notes = s.Notes,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt ?? s.CreatedAt
        });
    }

    public async Task<bool> IsTimeSlotBlockedAsync(Guid therapistId, DateTime dateTime)
    {
        return await _scheduleRepository.IsTimeSlotBlockedAsync(therapistId, dateTime);
    }
}
