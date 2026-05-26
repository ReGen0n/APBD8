namespace APBD8.Services;

using APBD8.DTOs;

public interface IPCService
{
    Task<List<PCResponseDto>> GetAllAsync();
    Task<List<PCComponentResponseDto>?> GetComponentsByPCIdAsync(int id);
    Task<PCResponseDto> CreateAsync(PCRequestDto dto);
    Task<PCResponseDto?> UpdateAsync(int id, PCRequestDto dto);
    Task<bool> DeleteAsync(int id);
}