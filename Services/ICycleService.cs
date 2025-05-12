using System;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;

namespace FemFitPlus.Services;

public interface ICycleService
{
    Task<CycleDto> CreateCycleAsync(CycleCreateDto cycleCreateDto);
    Task<CycleDto> GetCycleById(string cycleId);
    Task<CycleDto> UpdateCycle(string cycleId, CycleUpdateDto cycleUpdateDto);
    Task DeleteCycle(string cycleId);
    Task<List<CycleDto>> GetAllCycles();
    Task<List<CycleDto>> GetCyclesByUserId(string userId);
    Task<List<CycleDto>> FilterCycles(CycleFilter filter);
}
