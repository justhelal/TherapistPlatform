using TherapistApi.Application.Interfaces;
using TherapistApi.Domain.Interfaces;
using TherapistApi.Domain.Entities;
using TherapistApi.Domain.DTOs;
using TherapistApi.Domain.Enums;
using Shared.Common;

namespace TherapistApi.Application.Services;

public class TherapistService : ITherapistService
{
    private readonly ITherapistRepository _therapistRepository;

    public TherapistService(ITherapistRepository therapistRepository)
    {
        _therapistRepository = therapistRepository;
    }

    public async Task<ApiResponse<IEnumerable<TherapistDto>>> GetAllTherapistsAsync()
    {
        try
        {
            var therapists = await _therapistRepository.GetAllAsync();
            var therapistDtos = therapists.Select(MapToDto);
            return ApiResponse<IEnumerable<TherapistDto>>.SuccessResult(therapistDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TherapistDto>>.ErrorResult($"Error retrieving therapists: {ex.Message}");
        }
    }

    public async Task<ApiResponse<TherapistDto>> GetTherapistByIdAsync(Guid id)
    {
        try
        {
            var therapist = await _therapistRepository.GetByIdAsync(id);
            if (therapist == null)
                return ApiResponse<TherapistDto>.ErrorResult("Therapist not found");

            return ApiResponse<TherapistDto>.SuccessResult(MapToDto(therapist));
        }
        catch (Exception ex)
        {
            return ApiResponse<TherapistDto>.ErrorResult($"Error retrieving therapist: {ex.Message}");
        }
    }

    public async Task<ApiResponse<TherapistDto>> CreateTherapistAsync(CreateTherapistDto createTherapistDto)
    {
        try
        {
            // Check if email already exists
            var existingTherapist = await _therapistRepository.GetByEmailAsync(createTherapistDto.Email);
            if (existingTherapist != null)
                return ApiResponse<TherapistDto>.ErrorResult("A therapist with this email already exists");

            // Check if license number already exists
            existingTherapist = await _therapistRepository.GetByLicenseNumberAsync(createTherapistDto.LicenseNumber);
            if (existingTherapist != null)
                return ApiResponse<TherapistDto>.ErrorResult("A therapist with this license number already exists");

            var therapist = MapToEntity(createTherapistDto);
            therapist.Status = TherapistStatus.Active;
            
            var createdTherapist = await _therapistRepository.AddAsync(therapist);
            return ApiResponse<TherapistDto>.SuccessResult(MapToDto(createdTherapist), "Therapist created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<TherapistDto>.ErrorResult($"Error creating therapist: {ex.Message}");
        }
    }

    public async Task<ApiResponse<TherapistDto>> UpdateTherapistAsync(Guid id, CreateTherapistDto updateTherapistDto)
    {
        try
        {
            var existingTherapist = await _therapistRepository.GetByIdAsync(id);
            if (existingTherapist == null)
                return ApiResponse<TherapistDto>.ErrorResult("Therapist not found");

            // Update properties
            existingTherapist.FirstName = updateTherapistDto.FirstName;
            existingTherapist.LastName = updateTherapistDto.LastName;
            existingTherapist.Email = updateTherapistDto.Email;
            existingTherapist.PhoneNumber = updateTherapistDto.PhoneNumber;
            existingTherapist.LicenseNumber = updateTherapistDto.LicenseNumber;
            existingTherapist.Specialization = updateTherapistDto.Specialization;
            existingTherapist.LicenseExpiryDate = updateTherapistDto.LicenseExpiryDate;
            existingTherapist.YearsOfExperience = updateTherapistDto.YearsOfExperience;
            existingTherapist.Biography = updateTherapistDto.Biography;
            existingTherapist.HourlyRate = updateTherapistDto.HourlyRate;
            existingTherapist.Address = updateTherapistDto.Address;
            existingTherapist.City = updateTherapistDto.City;
            existingTherapist.State = updateTherapistDto.State;
            existingTherapist.ZipCode = updateTherapistDto.ZipCode;
            existingTherapist.Country = updateTherapistDto.Country;
            existingTherapist.UpdatedAt = DateTime.UtcNow;

            var updatedTherapist = await _therapistRepository.UpdateAsync(existingTherapist);
            return ApiResponse<TherapistDto>.SuccessResult(MapToDto(updatedTherapist), "Therapist updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<TherapistDto>.ErrorResult($"Error updating therapist: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeleteTherapistAsync(Guid id)
    {
        try
        {
            var exists = await _therapistRepository.ExistsAsync(id);
            if (!exists)
                return ApiResponse<bool>.ErrorResult("Therapist not found");

            await _therapistRepository.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResult(true, "Therapist deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult($"Error deleting therapist: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<TherapistDto>>> GetTherapistsBySpecializationAsync(int specialization)
    {
        try
        {
            var therapists = await _therapistRepository.GetBySpecializationAsync(specialization);
            var therapistDtos = therapists.Select(MapToDto);
            return ApiResponse<IEnumerable<TherapistDto>>.SuccessResult(therapistDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TherapistDto>>.ErrorResult($"Error retrieving therapists by specialization: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<TherapistDto>>> GetActiveTherapistsAsync()
    {
        try
        {
            var therapists = await _therapistRepository.GetActiveTherapistsAsync();
            var therapistDtos = therapists.Select(MapToDto);
            return ApiResponse<IEnumerable<TherapistDto>>.SuccessResult(therapistDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<TherapistDto>>.ErrorResult($"Error retrieving active therapists: {ex.Message}");
        }
    }

    private static TherapistDto MapToDto(Therapist therapist)
    {
        return new TherapistDto
        {
            Id = therapist.Id,
            FirstName = therapist.FirstName,
            LastName = therapist.LastName,
            Email = therapist.Email,
            PhoneNumber = therapist.PhoneNumber,
            LicenseNumber = therapist.LicenseNumber,
            Specialization = therapist.Specialization,
            Status = therapist.Status,
            LicenseExpiryDate = therapist.LicenseExpiryDate,
            YearsOfExperience = therapist.YearsOfExperience,
            Biography = therapist.Biography,
            HourlyRate = therapist.HourlyRate,
            Address = therapist.Address,
            City = therapist.City,
            State = therapist.State,
            ZipCode = therapist.ZipCode,
            Country = therapist.Country,
            FullName = therapist.FullName,
            IsLicenseValid = therapist.IsLicenseValid
        };
    }

    private static Therapist MapToEntity(CreateTherapistDto dto)
    {
        return new Therapist
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            LicenseNumber = dto.LicenseNumber,
            Specialization = dto.Specialization,
            LicenseExpiryDate = dto.LicenseExpiryDate,
            YearsOfExperience = dto.YearsOfExperience,
            Biography = dto.Biography,
            HourlyRate = dto.HourlyRate,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Country = dto.Country
        };
    }
}
