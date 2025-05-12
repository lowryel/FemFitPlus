using System.Security.Claims;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services;
using FemFitPlus.Services.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mlapper.Auto.Mapper;

namespace FemFitPlus.Controllers;

[Route("profile")]
[ApiController]
public class ProfileController(IMapper mapper, IProfileService profileService) : ControllerBase
{
    // private readonly IUserAuthService _service;
    private readonly IMapper _mapper = mapper;
    // private readonly IJwtTokenService _jwtTokenService;
    private readonly IProfileService _profileService = profileService;

    // [Authorize(Roles = "Admin")]
    [HttpGet()]
    public async Task<IActionResult> Query(ProfileFilter filter)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
        // filter.UserId = userId;
        var profile = await _profileService.Query(filter);
        return Ok(profile);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(ProfileCreateDto createDto)
    {
        try
        {
            _mapper.Map<ProfileCreateDto, Profile>(createDto);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            // createDto.UserId = userId;
            var profile = await _profileService.Create(createDto);
            return Ok(profile);
        }
        catch (Exception)
        { 
            throw new InvalidOperationException("An error occurred while creating the profile. Please try again.");
        }
    }

    [HttpGet("info")]
    public async Task<IActionResult> Info(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
        var profile = await _profileService.Info(id);
        return Ok(profile);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(string id, [FromBody]ProfileCreateDto createDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        System.Console.WriteLine($"UserId: {userId} Id: {id}");
        if (userId == null)
        {
            return Unauthorized();
        }
        createDto.UserId = userId;
        var profile = await _profileService.Update(userId, createDto);
        return Ok(profile);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != id)
        {
            return Unauthorized();
        }
        var profile = await _profileService.Delete(id);
        return Ok(profile);
    }
}

