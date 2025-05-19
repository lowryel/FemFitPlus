using System;
using System.Linq;
using FemFitPlus.Data;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mlapper.Auto.Mapper;

namespace FemFitPlus.Services;

public class ProfileService(IMapper mapper, FemFitPlusContext context) : IProfileService
{
    private readonly FemFitPlusContext _context = context;

    private readonly IMapper _mapper = mapper;

    public async Task<ProfileCreateDto> Create(ProfileCreateDto createDto)
    {
        try
        {
            // First verify the user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == createDto.UserId);
            if (!userExists)
            {
                throw new InvalidOperationException("User does not exist");
            }

            // Then check if profile already exists
            if (await _context.Profiles.AnyAsync(p => p.UserId == createDto.UserId))
            {
                throw new InvalidOperationException("Profile already exists for this user");
            }

            var profile = _mapper.Map<ProfileCreateDto, Profile>(createDto);
            profile.Id = Guid.NewGuid().ToString();
            profile.UserId = createDto.UserId!;
            profile.CreatedAt = DateTime.UtcNow;
            profile.UpdatedAt = DateTime.UtcNow;

            _context.Profiles.Add(profile);
            await _context.SaveChangesAsync();

            return _mapper.Map<Profile, ProfileCreateDto>(profile);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"An error occurred while creating the profile. Please try again. {ex.Message}");
        }
    }

    public Task<bool> Delete(string id)
    {
        try
        {
            var profile = _context.Profiles
                .Include(p => p.Femfituser)
                .FirstOrDefault(p => p.Id == id) ?? throw new InvalidOperationException("Profile not found");
            _context.Profiles.Remove(profile);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while deleting the profile. Please try again.", ex);
        }
    }

    public Task<ProfileDto> Info(string id)
    {
        try
        {
            var profile = _context.Profiles
                .Include(p => p.Femfituser)
                .FirstOrDefault(p => p.Id == id) ?? throw new InvalidOperationException("Profile not found");
            return Task.FromResult(new ProfileDto(
                profile.Id,
                profile.UserId,
                profile.HeightCm,
                profile.WeightKg,
                profile.FitnessGoal,
                profile.CycleLength,
                profile.PeriodLength,
                profile.LifestyleNotes,
                profile.PreferredWorkoutType
            ));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while retrieving the profile. Please try again.", ex);
        }
    }

    public async Task<List<ProfileDto>> Query(ProfileFilter filter)
    {
        var query = _context.Profiles
            .Where(p => p.Id != null)
            .Include(p => p.Femfituser)
            .AsQueryable();

        // Apply filter conditions
        if (filter != null)
        {
            query = filter.BuildQuery(query);
        }

        // Apply pagination after filtering
        query = query.Skip(filter?.Skip() ?? 0).Take(filter?.Size ?? 10);

        var profiles = await query
            .Select(p => new ProfileDto(
                p.Id,
                p.UserId,
                p.HeightCm,
                p.WeightKg,
                p.FitnessGoal,
                p.CycleLength,
                p.PeriodLength,
                p.LifestyleNotes,
                p.PreferredWorkoutType
            ))
        .ToListAsync();

        return profiles;
    }

    public Task<bool> Update(string id, ProfileCreateDto createDto)
    {
        try
        {
            var profile = _context.Profiles
                .Include(p => p.Femfituser)
                .FirstOrDefault(p => p.Id == id) ?? throw new InvalidOperationException("Profile not found");

            _mapper.Map(createDto, profile);
            profile.UpdatedAt = DateTime.UtcNow;
            _context.Profiles.Update(profile);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("An error occurred while updating the profile. Please try again.", ex);
        }
    }
}
