using PatientApi.Domain.Entities;
using PatientApi.Domain.Enums;
using Shared.Common;

namespace PatientApi.Domain.Interfaces;

public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetByEmailAsync(string email);
    Task<IEnumerable<Patient>> GetActivePatientsAsync();
    Task<IEnumerable<Patient>> GetPatientsByStatusAsync(PatientStatus status);
    Task<Patient?> GetPatientWithAppointmentsAsync(Guid patientId);
}
