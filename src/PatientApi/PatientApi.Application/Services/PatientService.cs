using PatientApi.Application.DTOs;
using PatientApi.Application.Interfaces;
using PatientApi.Domain.Entities;
using PatientApi.Domain.Enums;
using PatientApi.Domain.Interfaces;
using Shared.Common;
using MassTransit;
using Shared.Events;

namespace PatientApi.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public PatientService(IPatientRepository patientRepository, IPublishEndpoint publishEndpoint)
    {
        _patientRepository = patientRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ApiResponse<PatientDto>> GetPatientByIdAsync(Guid id)
    {
        try
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
                return ApiResponse<PatientDto>.ErrorResult("Patient not found");

            return ApiResponse<PatientDto>.SuccessResult(MapToDto(patient));
        }
        catch (Exception ex)
        {
            return ApiResponse<PatientDto>.ErrorResult($"Error retrieving patient: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PatientDto>> CreatePatientAsync(CreatePatientDto createPatientDto)
    {
        try
        {
            // Check if email already exists
            var existingPatient = await _patientRepository.GetByEmailAsync(createPatientDto.Email);
            if (existingPatient != null)
                return ApiResponse<PatientDto>.ErrorResult("A patient with this email already exists");

            var patient = MapToEntity(createPatientDto);
            patient.Status = PatientStatus.Active;
            
            var createdPatient = await _patientRepository.AddAsync(patient);

            // Publish PatientCreatedEvent
            await _publishEndpoint.Publish(new PatientCreatedEvent
            {
                PatientId = createdPatient.Id,
                FirstName = createdPatient.FirstName,
                LastName = createdPatient.LastName,
                Email = createdPatient.Email,
                DateOfBirth = createdPatient.DateOfBirth,
                CreatedAt = createdPatient.CreatedAt
            });

            return ApiResponse<PatientDto>.SuccessResult(MapToDto(createdPatient), "Patient created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PatientDto>.ErrorResult($"Error creating patient: {ex.Message}");
        }
    }

    private static PatientDto MapToDto(Patient patient)
    {
        return new PatientDto
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Email = patient.Email,
            PhoneNumber = patient.PhoneNumber,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            Status = patient.Status,
            Address = patient.Address,
            City = patient.City,
            State = patient.State,
            ZipCode = patient.ZipCode,
            Country = patient.Country,
            EmergencyContactName = patient.EmergencyContactName,
            EmergencyContactPhone = patient.EmergencyContactPhone,
            InsuranceProvider = patient.InsuranceProvider,
            InsurancePolicyNumber = patient.InsurancePolicyNumber,
            MedicalHistory = patient.MedicalHistory,
            CurrentMedications = patient.CurrentMedications,
            Allergies = patient.Allergies,
            FullName = patient.FullName,
            Age = patient.Age
        };
    }

    private static Patient MapToEntity(CreatePatientDto dto)
    {
        return new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Country = dto.Country,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactPhone = dto.EmergencyContactPhone,
            InsuranceProvider = dto.InsuranceProvider,
            InsurancePolicyNumber = dto.InsurancePolicyNumber,
            MedicalHistory = dto.MedicalHistory,
            CurrentMedications = dto.CurrentMedications,
            Allergies = dto.Allergies
        };
    }
}
