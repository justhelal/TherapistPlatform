using PatientApi.Application.DTOs;
using Shared.Common;

namespace PatientApi.Application.Interfaces;

public interface IPatientService
{
    Task<ApiResponse<PatientDto>> GetPatientByIdAsync(Guid id);
    Task<ApiResponse<PatientDto>> CreatePatientAsync(CreatePatientDto createPatientDto);
}
