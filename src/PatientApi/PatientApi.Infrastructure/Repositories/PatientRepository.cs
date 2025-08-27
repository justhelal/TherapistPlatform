using Microsoft.EntityFrameworkCore;
using PatientApi.Domain.Entities;
using PatientApi.Domain.Interfaces;
using PatientApi.Infrastructure.Data;
using Shared.Common;

namespace PatientApi.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly PatientDbContext _context;

    public PatientRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(Guid id)
    {
        return await _context.Patients.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients.Where(p => !p.IsDeleted).ToListAsync();
    }

    public async Task<Patient> AddAsync(Patient entity)
    {
        _context.Patients.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Patient> UpdateAsync(Patient entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Patients.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient != null)
        {
            patient.IsDeleted = true;
            patient.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Patients.AnyAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<Patient?> GetByEmailAsync(string email)
    {
        return await _context.Patients.FirstOrDefaultAsync(p => p.Email == email && !p.IsDeleted);
    }

    public async Task<IEnumerable<Patient>> GetActivePatientsAsync()
    {
        return await _context.Patients
            .Where(p => !p.IsDeleted && p.Status == Domain.Enums.PatientStatus.Active)
            .ToListAsync();
    }

    public async Task<IEnumerable<Patient>> GetPatientsByStatusAsync(Domain.Enums.PatientStatus status)
    {
        return await _context.Patients
            .Where(p => !p.IsDeleted && p.Status == status)
            .ToListAsync();
    }

    public async Task<Patient?> GetPatientWithAppointmentsAsync(Guid patientId)
    {
        return await _context.Patients
            .Include(p => p.Appointments)
            .FirstOrDefaultAsync(p => p.Id == patientId && !p.IsDeleted);
    }
}
