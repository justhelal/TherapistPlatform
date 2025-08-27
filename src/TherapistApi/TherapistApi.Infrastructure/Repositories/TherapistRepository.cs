using Microsoft.EntityFrameworkCore;
using TherapistApi.Domain.Entities;
using TherapistApi.Domain.Interfaces;
using TherapistApi.Domain.Enums;
using TherapistApi.Infrastructure.Data;

namespace TherapistApi.Infrastructure.Repositories;

public class TherapistRepository : ITherapistRepository
{
    private readonly TherapistDbContext _context;

    public TherapistRepository(TherapistDbContext context)
    {
        _context = context;
    }

    public async Task<Therapist?> GetByIdAsync(Guid id)
    {
        return await _context.Therapists
            .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
    }

    public async Task<IEnumerable<Therapist>> GetAllAsync()
    {
        return await _context.Therapists
            .Where(t => !t.IsDeleted)
            .OrderBy(t => t.LastName)
            .ThenBy(t => t.FirstName)
            .ToListAsync();
    }

    public async Task<Therapist> AddAsync(Therapist entity)
    {
        _context.Therapists.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Therapist> UpdateAsync(Therapist entity)
    {
        _context.Therapists.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var therapist = await _context.Therapists.FindAsync(id);
        if (therapist != null)
        {
            therapist.IsDeleted = true;
            therapist.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Therapists
            .AnyAsync(t => t.Id == id && !t.IsDeleted);
    }

    public async Task<IEnumerable<Therapist>> GetBySpecializationAsync(int specialization)
    {
        return await _context.Therapists
            .Where(t => !t.IsDeleted && (int)t.Specialization == specialization)
            .OrderBy(t => t.LastName)
            .ThenBy(t => t.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Therapist>> GetActiveTherapistsAsync()
    {
        return await _context.Therapists
            .Where(t => !t.IsDeleted && t.Status == TherapistStatus.Active)
            .OrderBy(t => t.LastName)
            .ThenBy(t => t.FirstName)
            .ToListAsync();
    }

    public async Task<Therapist?> GetByLicenseNumberAsync(string licenseNumber)
    {
        return await _context.Therapists
            .FirstOrDefaultAsync(t => t.LicenseNumber == licenseNumber && !t.IsDeleted);
    }

    public async Task<Therapist?> GetByEmailAsync(string email)
    {
        return await _context.Therapists
            .FirstOrDefaultAsync(t => t.Email == email && !t.IsDeleted);
    }
}
