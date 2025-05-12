using System;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;

namespace FemFitPlus.Services;

public interface IProfileService
{
    Task<ProfileCreateDto> Create(ProfileCreateDto createDto);
    Task<ProfileDto> Info(string id);
    // Task<ProfileDto[]> GetProfiles(ProfileCreateDto createDto);
    Task<bool> Update(string id, ProfileCreateDto createDto);
    Task<bool> Delete(string id);
    Task<List<ProfileDto>> Query(ProfileFilter filter);
}
