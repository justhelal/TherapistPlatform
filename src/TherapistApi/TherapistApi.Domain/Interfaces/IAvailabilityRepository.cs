using TherapistApi.Domain.Entities;
using Shared.Common;

namespace TherapistApi.Domain.Interfaces;

public interface IAvailabilityRepository : IRepository<Availability>
{
    Task<IEnumerable<Availability>> GetByTherapistIdAsync(Guid therapistId);
    Task<IEnumerable<Availability>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}
