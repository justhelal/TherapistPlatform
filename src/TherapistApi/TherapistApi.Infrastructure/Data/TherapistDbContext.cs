using Microsoft.EntityFrameworkCore;
using TherapistApi.Domain.Entities;

namespace TherapistApi.Infrastructure.Data;

public class TherapistDbContext : DbContext
{
    public TherapistDbContext(DbContextOptions<TherapistDbContext> options) : base(options)
    {
    }

    public DbSet<Therapist> Therapists { get; set; }
    public DbSet<Availability> Availabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Therapist entity
        modelBuilder.Entity<Therapist>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.LicenseNumber).IsUnique();
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Biography).HasMaxLength(2000);
            entity.Property(e => e.HourlyRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.State).HasMaxLength(100);
            entity.Property(e => e.ZipCode).HasMaxLength(20);
            entity.Property(e => e.Country).HasMaxLength(100);
        });

        // Configure Availability entity
        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Therapist)
                  .WithMany()
                  .HasForeignKey(e => e.TherapistId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
