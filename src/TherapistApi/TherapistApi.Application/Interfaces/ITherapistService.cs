using TherapistApi.Domain.DTOs;
using Shared.Common;

namespace TherapistApi.Application.Interfaces;

public interface ITherapistService
{
    Task<ApiResponse<IEnumerable<TherapistDto>>> GetAllTherapistsAsync();
    Task<ApiResponse<TherapistDto>> GetTherapistByIdAsync(Guid id);
    Task<ApiResponse<TherapistDto>> CreateTherapistAsync(CreateTherapistDto createTherapistDto);
    Task<ApiResponse<TherapistDto>> UpdateTherapistAsync(Guid id, CreateTherapistDto updateTherapistDto);
    Task<ApiResponse<bool>> DeleteTherapistAsync(Guid id);
    Task<ApiResponse<IEnumerable<TherapistDto>>> GetTherapistsBySpecializationAsync(int specialization);
    Task<ApiResponse<IEnumerable<TherapistDto>>> GetActiveTherapistsAsync();
}
