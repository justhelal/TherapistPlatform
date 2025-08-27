using PatientApi.Application.DTOs;
using Shared.Common;

namespace PatientApi.Application.Interfaces;

public interface IPatientService
{
    Task<ApiResponse<IEnumerable<PatientDto>>> GetAllPatientsAsync();
    Task<ApiResponse<PatientDto>> GetPatientByIdAsync(Guid id);
    Task<ApiResponse<PatientDto>> CreatePatientAsync(CreatePatientDto createPatientDto);
    Task<ApiResponse<PatientDto>> UpdatePatientAsync(Guid id, CreatePatientDto updatePatientDto);
    Task<ApiResponse<bool>> DeletePatientAsync(Guid id);
    Task<ApiResponse<IEnumerable<PatientDto>>> GetActivePatientsAsync();
    Task<ApiResponse<PatientDto>> GetPatientWithAppointmentsAsync(Guid id);
}
