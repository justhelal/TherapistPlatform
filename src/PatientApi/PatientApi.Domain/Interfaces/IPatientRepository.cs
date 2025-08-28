using PatientApi.Domain.Entities;
using PatientApi.Domain.Enums;
using Shared.Common;

namespace PatientApi.Domain.Interfaces;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByEmailAsync(string email);
    new Task<List<Patient>> GetAllAsync();
}
