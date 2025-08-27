using Shared.Common;
using TherapistApi.Domain.Enums;

namespace TherapistApi.Domain.Entities;

public class Therapist : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public TherapistSpecialization Specialization { get; set; }
    public TherapistStatus Status { get; set; }
    public DateTime LicenseExpiryDate { get; set; }
    public int YearsOfExperience { get; set; }
    public string Biography { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";
    public bool IsLicenseValid => LicenseExpiryDate > DateTime.UtcNow;
}
