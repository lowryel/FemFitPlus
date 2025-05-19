using System;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;

namespace FemFitPlus.Services;

public interface ICycleService
{
    Task<CycleCreateDto> CreateCycleAsync(CycleCreateDto cycleCreateDto);
    Task<CycleDto> GetCycleByIdAsync(string cycleId, string userId);
    Task<CycleDto> UpdateCycleAsync(string cycleId, CycleUpdateDto cycleUpdateDto);
    Task<bool> DeleteCycleAsync(string cycleId, string userId);
    Task<List<CycleDto>> GetAllCyclesAsync();
    Task<List<CycleDto>> GetCyclesByUserIdAsync(string userId);
    Task<List<CycleDto>> Query(CycleFilter filter);
}
