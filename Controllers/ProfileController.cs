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
    private readonly IMapper _mapper = mapper;
    private readonly IProfileService _profileService = profileService;

    [Authorize(Roles = "Admin")]
    [HttpGet()]
    public async Task<IActionResult> Query(ProfileFilter filter)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
        // filter.UserId = userId;
        try
        {
            // Optionally validate filter properties here to prevent injection via filter fields
            List<ProfileDto> profile = await _profileService.Query(filter);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            // Log the exception as needed
            return StatusCode(500, $"An error occurred while querying profiles. Please try again. {ex.Message}");
        }
    }
    
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create(ProfileCreateDto createDto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            createDto.UserId = userId;
            var profile = await _profileService.Create(createDto);
            return Ok(profile);
        }
        catch (Exception)
        {
            return StatusCode(500, $"An error occurred while creating the profile. Please try again");
        }
    }

    [Authorize]
    [HttpGet("info")]
    public async Task<IActionResult> Info(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return StatusCode(403, "User not authenticated");
        }
        if (id == null )
        {
            return BadRequest("Profile ID cannot be null");
        }
        try
        {
            ProfileDto profile = await _profileService.Info(id);
            return Ok(profile);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while retrieving profile information.");
        }
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> Update(string id, [FromBody] ProfileCreateDto createDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
        createDto.UserId = userId;
        var profile = await _profileService.Update(id, createDto);
        return Ok(profile);
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != id)
        {
            return Unauthorized();
        }
        try
        {
            var profile = await _profileService.Delete(id);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting the profile. Please try again. {ex.Message}");
        }
    }
}

