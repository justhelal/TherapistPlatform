using TherapistApi.Application.DTOs;

namespace TherapistApi.Application.Interfaces;

public interface ITherapistScheduleService
{
    Task<IEnumerable<TherapistScheduleDto>> GetTherapistScheduleAsync(Guid therapistId);
    Task<bool> IsTimeSlotBlockedAsync(Guid therapistId, DateTime dateTime);
}
