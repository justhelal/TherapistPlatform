using Microsoft.EntityFrameworkCore;
using TherapistApi.Domain.Entities;
using TherapistApi.Domain.Interfaces;
using TherapistApi.Infrastructure.Data;

namespace TherapistApi.Infrastructure.Repositories;

public class TherapistScheduleRepository : ITherapistScheduleRepository
{
    private readonly TherapistDbContext _context;

    public TherapistScheduleRepository(TherapistDbContext context)
    {
        _context = context;
    }

    public async Task<TherapistSchedule?> GetByIdAsync(Guid id)
    {
        return await _context.TherapistSchedules.FindAsync(id);
    }

    public async Task<IEnumerable<TherapistSchedule>> GetAllAsync()
    {
        return await _context.TherapistSchedules.ToListAsync();
    }

    public async Task<TherapistSchedule> AddAsync(TherapistSchedule entity)
    {
        _context.TherapistSchedules.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TherapistSchedule> UpdateAsync(TherapistSchedule entity)
    {
        _context.TherapistSchedules.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _context.TherapistSchedules.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.TherapistSchedules.AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<TherapistSchedule>> GetByTherapistIdAsync(Guid therapistId)
    {
        return await _context.TherapistSchedules
            .Where(s => s.TherapistId == therapistId)
            .OrderBy(s => s.AppointmentDateTime)
            .ToListAsync();
    }

    public async Task<bool> IsTimeSlotBlockedAsync(Guid therapistId, DateTime dateTime)
    {
        return await _context.TherapistSchedules
            .AnyAsync(s => s.TherapistId == therapistId && 
                          s.AppointmentDateTime == dateTime && 
                          s.IsBlocked);
    }
}
