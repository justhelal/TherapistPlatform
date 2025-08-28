using Microsoft.EntityFrameworkCore;
using PatientApi.Domain.Entities;
using PatientApi.Domain.Interfaces;
using PatientApi.Infrastructure.Data;

namespace PatientApi.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly PatientDbContext _context;

    public AppointmentRepository(PatientDbContext context)
    {
        _context = context;
    }

    public async Task<Appointment> AddAsync(Appointment entity)
    {
        _context.Appointments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // Keep minimal IRepository implementation for base functionality
    public async Task<Appointment?> GetByIdAsync(Guid id)
    {
        return await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<IEnumerable<Appointment>> GetAllAsync()
    {
        return await _context.Appointments.Where(a => !a.IsDeleted).ToListAsync();
    }

    public async Task<Appointment> UpdateAsync(Appointment entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _context.Appointments.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment != null)
        {
            appointment.IsDeleted = true;
            appointment.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Appointments.AnyAsync(a => a.Id == id && !a.IsDeleted);
    }

    public async Task<IEnumerable<Appointment>> GetByPatientIdAsync(Guid patientId)
    {
        return await _context.Appointments
            .Include(a => a.Patient)
            .Where(a => a.PatientId == patientId && !a.IsDeleted)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();
    }
}
