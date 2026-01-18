using System.Security.Claims;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services;
using FemFitPlus.Services.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mlapper.Auto.Mapper;

namespace FemFitPlus.Controllers;

[ApiController]
[Route("[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IProfileService _profileService;

    public ProfileController(IMapper mapper, IProfileService profileService)
    {
        _mapper = mapper;
        _profileService = profileService;
    }

    // [Authorize(Roles = "Admin")]
    [HttpGet("query")]
    public async Task<IActionResult> Query([FromQuery] ProfileFilter filter)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var profiles = await _profileService.Query(filter);
        return Ok(profiles);
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ProfileCreateDto createDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        createDto.UserId = userId;
        var profile = await _profileService.Create(createDto);
        return Ok(profile);
    }

    [Authorize]
    [HttpGet("info/{id}")]
    public async Task<IActionResult> Info([FromRoute] string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Forbid();

        var profile = await _profileService.Info(id);
        return Ok(profile);
    }

    [Authorize]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] ProfileCreateDto createDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        createDto.UserId = userId;
        var profile = await _profileService.Update(id, createDto);
        return Ok(profile);
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != id)
            return Unauthorized();

        var result = await _profileService.Delete(id);
        return Ok(result);
    }
}

