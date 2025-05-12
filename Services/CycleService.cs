using System;
using FemFitPlus.Data;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;
using FemFitPlus.Shared;
using Microsoft.IdentityModel.Tokens;
using Mlapper.Auto.Mapper;

namespace FemFitPlus.Services;

public class CycleService(FemFitPlusContext context, IMapper mapper) : ICycleService
{
    private readonly FemFitPlusContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<CycleDto> CreateCycleAsync(CycleCreateDto cycleCreateDto)
    {
        // Validation
        if (cycleCreateDto == null)
        {
            throw new ArgumentNullException(nameof(cycleCreateDto), "Cycle data cannot be null");
        }

        if (string.IsNullOrEmpty(cycleCreateDto.UserId))
        {
            throw new ArgumentException("User ID is required", nameof(cycleCreateDto.UserId));
        }

        // Validate dates - make sure EndDate is actually provided before comparing
        if (cycleCreateDto.EndDate.HasValue && cycleCreateDto.StartDate > cycleCreateDto.EndDate)
        {
            throw new ArgumentException("Start date cannot be after end date");
        }

        if (cycleCreateDto.EnergyLevel < 0 || cycleCreateDto.EnergyLevel > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(cycleCreateDto.EnergyLevel), "Energy level must be between 0 and 10");
        }

        // Validate phase value against allowed values (if needed)
        if (!string.IsNullOrEmpty(cycleCreateDto.Phase!.Value.ToString()) && !Helpers.IsValidPhase(cycleCreateDto.Phase.Value.ToString()))
        {
            throw new ArgumentException("Invalid phase value", nameof(cycleCreateDto.Phase));
        }

        // Sanitize inputs to prevent injection attacks
        cycleCreateDto.Symptoms = Helpers.SanitizeInput(cycleCreateDto.Symptoms!);
        cycleCreateDto.Mood = Helpers.SanitizeInput(cycleCreateDto.Mood!);

        // Use automapper instead of manual mapping
        var cycle = _mapper.Map<CycleCreateDto, Cycle>(cycleCreateDto);

        // Set audit fields
        cycle.CreatedAt = DateTime.UtcNow;
        cycle.UpdatedAt = DateTime.UtcNow;

        // Performance improvement: Use a single database call
        await _context.Cycles.AddAsync(cycle);
        await _context.SaveChangesAsync();

        // Return DTO using automapper
        return _mapper.Map<Cycle, CycleDto>(cycle);
    }

    public Task DeleteCycle(string cycleId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CycleDto>> FilterCycles(CycleFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<List<CycleDto>> GetAllCycles()
    {
        throw new NotImplementedException();
    }

    public Task<CycleDto> GetCycleById(string cycleId)
    {
        throw new NotImplementedException();
    }

    public Task<List<CycleDto>> GetCyclesByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<CycleDto> UpdateCycle(string cycleId, CycleUpdateDto cycleUpdateDto)
    {
        throw new NotImplementedException();
    }
}
