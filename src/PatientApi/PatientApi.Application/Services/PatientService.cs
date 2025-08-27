using PatientApi.Application.DTOs;
using PatientApi.Application.Interfaces;
using PatientApi.Domain.Entities;
using PatientApi.Domain.Enums;
using PatientApi.Domain.Interfaces;
using Shared.Common;

namespace PatientApi.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<ApiResponse<IEnumerable<PatientDto>>> GetAllPatientsAsync()
    {
        try
        {
            var patients = await _patientRepository.GetAllAsync();
            var patientDtos = patients.Select(MapToDto);
            return ApiResponse<IEnumerable<PatientDto>>.SuccessResult(patientDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<PatientDto>>.ErrorResult($"Error retrieving patients: {ex.Message}");
        }
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
            return ApiResponse<PatientDto>.SuccessResult(MapToDto(createdPatient), "Patient created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PatientDto>.ErrorResult($"Error creating patient: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PatientDto>> UpdatePatientAsync(Guid id, CreatePatientDto updatePatientDto)
    {
        try
        {
            var existingPatient = await _patientRepository.GetByIdAsync(id);
            if (existingPatient == null)
                return ApiResponse<PatientDto>.ErrorResult("Patient not found");

            // Update properties
            existingPatient.FirstName = updatePatientDto.FirstName;
            existingPatient.LastName = updatePatientDto.LastName;
            existingPatient.Email = updatePatientDto.Email;
            existingPatient.PhoneNumber = updatePatientDto.PhoneNumber;
            existingPatient.DateOfBirth = updatePatientDto.DateOfBirth;
            existingPatient.Gender = updatePatientDto.Gender;
            existingPatient.Address = updatePatientDto.Address;
            existingPatient.City = updatePatientDto.City;
            existingPatient.State = updatePatientDto.State;
            existingPatient.ZipCode = updatePatientDto.ZipCode;
            existingPatient.Country = updatePatientDto.Country;
            existingPatient.EmergencyContactName = updatePatientDto.EmergencyContactName;
            existingPatient.EmergencyContactPhone = updatePatientDto.EmergencyContactPhone;
            existingPatient.InsuranceProvider = updatePatientDto.InsuranceProvider;
            existingPatient.InsurancePolicyNumber = updatePatientDto.InsurancePolicyNumber;
            existingPatient.MedicalHistory = updatePatientDto.MedicalHistory;
            existingPatient.CurrentMedications = updatePatientDto.CurrentMedications;
            existingPatient.Allergies = updatePatientDto.Allergies;

            var updatedPatient = await _patientRepository.UpdateAsync(existingPatient);
            return ApiResponse<PatientDto>.SuccessResult(MapToDto(updatedPatient), "Patient updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PatientDto>.ErrorResult($"Error updating patient: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeletePatientAsync(Guid id)
    {
        try
        {
            var exists = await _patientRepository.ExistsAsync(id);
            if (!exists)
                return ApiResponse<bool>.ErrorResult("Patient not found");

            await _patientRepository.DeleteAsync(id);
            return ApiResponse<bool>.SuccessResult(true, "Patient deleted successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult($"Error deleting patient: {ex.Message}");
        }
    }

    public async Task<ApiResponse<IEnumerable<PatientDto>>> GetActivePatientsAsync()
    {
        try
        {
            var patients = await _patientRepository.GetActivePatientsAsync();
            var patientDtos = patients.Select(MapToDto);
            return ApiResponse<IEnumerable<PatientDto>>.SuccessResult(patientDtos);
        }
        catch (Exception ex)
        {
            return ApiResponse<IEnumerable<PatientDto>>.ErrorResult($"Error retrieving active patients: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PatientDto>> GetPatientWithAppointmentsAsync(Guid id)
    {
        try
        {
            var patient = await _patientRepository.GetPatientWithAppointmentsAsync(id);
            if (patient == null)
                return ApiResponse<PatientDto>.ErrorResult("Patient not found");

            return ApiResponse<PatientDto>.SuccessResult(MapToDto(patient));
        }
        catch (Exception ex)
        {
            return ApiResponse<PatientDto>.ErrorResult($"Error retrieving patient with appointments: {ex.Message}");
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
