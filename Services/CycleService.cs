using System;
using FemFitPlus.Data;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;
using FemFitPlus.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Mlapper.Auto.Mapper;

namespace FemFitPlus.Services;

public class CycleService(FemFitPlusContext context, IMapper mapper, IMemoryCache cache) : ICycleService
{
    private readonly FemFitPlusContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IMemoryCache _cache = cache;

    // Cache configuration
    private const string CACHE_KEY_ALL_PRODUCTS = "all_products";
    private const string CACHE_KEY_PRODUCT_PREFIX = "product_";
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(30);
    private readonly TimeSpan _shortCacheExpiration = TimeSpan.FromMinutes(5);


    public async Task<CycleCreateDto> CreateCycleAsync(CycleCreateDto cycleCreateDto)
    {
        ArgumentNullException.ThrowIfNull(cycleCreateDto);

        if (string.IsNullOrEmpty(cycleCreateDto.UserId))
            throw new ArgumentException("User ID is required", nameof(cycleCreateDto.UserId));

        if (cycleCreateDto.EndDate.HasValue && cycleCreateDto.StartDate > cycleCreateDto.EndDate)
            throw new ArgumentException("Start date cannot be after end date");

        if (cycleCreateDto.EnergyLevel < 0 || cycleCreateDto.EnergyLevel > 10)
            throw new ArgumentOutOfRangeException(nameof(cycleCreateDto.EnergyLevel), "Must be between 0 and 10");

        if (cycleCreateDto.Phase.HasValue && !Helpers.IsValidPhase(cycleCreateDto.Phase.Value.ToString()))
            throw new ArgumentException("Invalid phase value", nameof(cycleCreateDto.Phase));

        if (cycleCreateDto.Symptoms != null)
        {
            cycleCreateDto.Symptoms = [.. cycleCreateDto.Symptoms.Select(s => Helpers.SanitizeInput(s))];
        }

        if (cycleCreateDto.Mood != null)
        {
            cycleCreateDto.Mood = [.. cycleCreateDto.Mood.Select(m => Helpers.SanitizeInput(m))];
        }

        var newCycle = _mapper.Map<CycleCreateDto, Cycle>(cycleCreateDto);
        newCycle.Id = Guid.NewGuid().ToString();
        newCycle.UserId = cycleCreateDto.UserId;
        newCycle.Phase = cycleCreateDto.Phase;
        newCycle.EnergyLevel = cycleCreateDto.EnergyLevel;
        newCycle.StartDate = cycleCreateDto.StartDate;
        newCycle.CreatedAt = newCycle.UpdatedAt = DateTime.UtcNow;

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var previousCycle = await _context.Cycles
                .Where(c => c.UserId == cycleCreateDto.UserId && c.EndDate == null)
                .OrderByDescending(c => c.StartDate)
                .FirstOrDefaultAsync();

            if (previousCycle != null)
            {
                previousCycle.EndDate = cycleCreateDto.StartDate.AddDays(-1);
                previousCycle.UpdatedAt = DateTime.UtcNow;

                _context.Cycles.Update(previousCycle);
            }

            await _context.Cycles.AddAsync(newCycle);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return _mapper.Map<Cycle, CycleCreateDto>(newCycle);
        }
        catch (Exception ex)
        {
            // Safely rollback only if still connected
            if (transaction?.GetDbTransaction()?.Connection != null)
            {
                await transaction.RollbackAsync();
            }

            throw new ApplicationException("An error occurred while creating the cycle", ex);
        }
    }



    public async Task<bool> DeleteCycleAsync(string cycleId, string userId)
    {
        if (string.IsNullOrEmpty(cycleId))
            throw new ArgumentException("Cycle ID is required", nameof(cycleId));

        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("User ID is required", nameof(userId));

        var cycle = await _context.Cycles.FindAsync(cycleId) ?? throw new KeyNotFoundException("Cycle not found");
        if (cycle.UserId != userId)
            throw new UnauthorizedAccessException("You do not have permission to delete this cycle");

        if (cycle == null)
        {
            // Invalidate both individual product cache and all products cache
            var individualCacheKey = $"{CACHE_KEY_PRODUCT_PREFIX}{cycle!.Id}";
            _cache.Remove(individualCacheKey);
            await InvalidateAllProductsCacheAsync();
        }

        _context.Cycles.Remove(cycle);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<CycleDto>> Query(CycleFilter filter)
    {
        if (_cache.TryGetValue(CACHE_KEY_ALL_PRODUCTS, out List<CycleDto>? cachedProducts))
        {
            return cachedProducts!;
        }
        var query = filter.BuildQuery(_context.Cycles.AsQueryable());

        query = query.Skip(filter?.Skip() ?? 0).Take(filter?.Size ?? 10);
        var cycles = await query.ToListAsync();

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheExpiration,
            SlidingExpiration = _shortCacheExpiration,
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(CACHE_KEY_ALL_PRODUCTS, cycles, cacheOptions);

        return [.. cycles.Select(c => new CycleDto
        {
            Id = c.Id,
            UserId = c.UserId,
            Phase = (CyclePhase)c.Phase!,
            EnergyLevel = c.EnergyLevel,
            Symptoms = c.Symptoms,
            Mood = c.Mood,
            StartDate = c.StartDate,
            EndDate = c.EndDate
        })];
    }

    public async Task<List<CycleDto>> GetCyclesByUserIdAsync(string userId)
    {
        if (_cache.TryGetValue(CACHE_KEY_ALL_PRODUCTS, out List<CycleDto>? cachedProducts))
        {
            return cachedProducts!;
        }

        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        var cycles = await _context.Cycles.Where(c => c.UserId == userId).ToListAsync();

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheExpiration,
            SlidingExpiration = _shortCacheExpiration,
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(CACHE_KEY_ALL_PRODUCTS, cycles, cacheOptions);

        return [.. cycles.Select(c => new CycleDto
        {
            Id = c.Id,
            UserId = c.UserId,
            Phase = (CyclePhase)c.Phase!,
            EnergyLevel = c.EnergyLevel,
            Symptoms = c.Symptoms,
            Mood = c.Mood,
            StartDate = c.StartDate,
            EndDate = c.EndDate
        })];
    }

    public async Task<CycleDto> GetCycleByIdAsync(string cycleId, string userId)
    {
        if (_cache.TryGetValue(CACHE_KEY_ALL_PRODUCTS, out CycleDto? cachedProducts))
        {
            return cachedProducts!;
        }

        if (string.IsNullOrEmpty(cycleId))
            throw new ArgumentException("Cycle ID is required", nameof(cycleId));

        if (string.IsNullOrEmpty(userId))
            throw new ArgumentException("User ID is required", nameof(userId));

        var cycle = await _context.Cycles.FindAsync(cycleId) ?? throw new KeyNotFoundException("Cycle not found");
        if (cycle.UserId != userId)
            throw new UnauthorizedAccessException("You do not have permission to access this cycle");

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheExpiration,
            SlidingExpiration = _shortCacheExpiration,
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(CACHE_KEY_ALL_PRODUCTS, cycle, cacheOptions);

        return _mapper.Map<Cycle, CycleDto>(cycle);
    }

    public async Task<CycleDto> UpdateCycleAsync(string cycleId, CycleUpdateDto cycleUpdateDto)
    {
        try
        {
            if (string.IsNullOrEmpty(cycleId))
                throw new ArgumentException("Cycle ID is required", nameof(cycleId));

            ArgumentNullException.ThrowIfNull(cycleUpdateDto);

            var cycle = await _context.Cycles.FindAsync(cycleId) ?? throw new KeyNotFoundException("Cycle not found");

            // Validation
            if (cycleUpdateDto.EnergyLevel < 0 || cycleUpdateDto.EnergyLevel > 10)
                throw new ArgumentOutOfRangeException(nameof(cycleUpdateDto.EnergyLevel));

            if (cycleUpdateDto.Phase.HasValue && !Helpers.IsValidPhase(cycleUpdateDto.Phase.Value.ToString()))
                throw new ArgumentException("Invalid phase value", nameof(cycleUpdateDto.Phase));

            // Sanitize
            if (cycleUpdateDto.Symptoms != null)
            {
                cycleUpdateDto.Symptoms = [.. cycleUpdateDto.Symptoms.Select(s => Helpers.SanitizeInput(s))];
            }

            if (cycleUpdateDto.Mood != null)
            {
                cycleUpdateDto.Mood = [.. cycleUpdateDto.Mood.Select(m => Helpers.SanitizeInput(m))];
            }

            _mapper.Map<Cycle, CycleUpdateDto>(cycle);
            cycle.Phase = cycleUpdateDto.Phase;
            cycle.EnergyLevel = cycleUpdateDto.EnergyLevel;
            cycle.Symptoms = cycleUpdateDto.Symptoms;
            cycle.Mood = cycleUpdateDto.Mood;
            cycle.UpdatedAt = DateTime.UtcNow;

            _context.Cycles.Update(cycle);
            await _context.SaveChangesAsync();

            return _mapper.Map<Cycle, CycleDto>(cycle);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while updating the cycle: {ex.Message}");
        }

    }

    public async Task<List<CycleDto>> GetAllCyclesAsync()
    {
        if (_cache.TryGetValue(CACHE_KEY_ALL_PRODUCTS, out List<CycleDto>? cachedProducts))
        {
            return cachedProducts!;
        }

        var cycles = await _context.Cycles.ToListAsync();

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _cacheExpiration,
            SlidingExpiration = _shortCacheExpiration,
            Priority = CacheItemPriority.Normal
        };

        _cache.Set(CACHE_KEY_ALL_PRODUCTS, cycles, cacheOptions);

        return [.. cycles.Select(c => new CycleDto
        {
            Id = c.Id,
            UserId = c.UserId,
            Phase = (CyclePhase)c.Phase!,
            EnergyLevel = c.EnergyLevel,
            Symptoms = c.Symptoms,
            Mood = c.Mood,
            StartDate = c.StartDate,
            EndDate = c.EndDate
        })];
    }

    public async Task InvalidateAllProductsCacheAsync()
    {
        _cache.Remove(CACHE_KEY_ALL_PRODUCTS);
        await Task.CompletedTask;
    }
}
