using TherapistApi.Domain.Entities;
using Shared.Common;

namespace TherapistApi.Domain.Interfaces;

public interface ITherapistRepository : IRepository<Therapist>
{
    Task<IEnumerable<Therapist>> GetBySpecializationAsync(int specialization);
    Task<IEnumerable<Therapist>> GetActiveTherapistsAsync();
    Task<Therapist?> GetByLicenseNumberAsync(string licenseNumber);
    Task<Therapist?> GetByEmailAsync(string email);
}
