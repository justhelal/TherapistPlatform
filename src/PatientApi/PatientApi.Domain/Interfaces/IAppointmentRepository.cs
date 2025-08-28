using PatientApi.Domain.Entities;
using Shared.Common;

namespace PatientApi.Domain.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetByPatientIdAsync(Guid patientId);
}
