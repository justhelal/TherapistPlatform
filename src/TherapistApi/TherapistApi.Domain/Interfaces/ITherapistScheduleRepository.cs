using TherapistApi.Domain.Entities;
using Shared.Common;

namespace TherapistApi.Domain.Interfaces;

public interface ITherapistScheduleRepository : IRepository<TherapistSchedule>
{
    Task<IEnumerable<TherapistSchedule>> GetByTherapistIdAsync(Guid therapistId);
    Task<bool> IsTimeSlotBlockedAsync(Guid therapistId, DateTime dateTime);
}
