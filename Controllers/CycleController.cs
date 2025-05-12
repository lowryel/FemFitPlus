using System;
using System.Security.Claims;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services;
using FemFitPlus.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FemFitPlus.Controllers;

[ApiController]
[Route("cycles")]
public class CycleController(ICycleService cycleService) : ControllerBase
{
    private readonly ICycleService _cycleService = cycleService;
    // Add methods for handling HTTP requests here
    // [HttpGet]
    // public IActionResult GetCycleById(int id)
    // {
    //     var cycle = _cycleService.GetCycleById(id);
    //     if (cycle == null)
    //     {
    //         return NotFound();
    //     }
    //     return Ok(cycle);
    // }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCycle([FromBody] CycleCreateDto cycleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Get authenticated user ID (use ClaimsPrincipal extension method for cleaner code)
        var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        cycleDto.UserId = userId;

        try
        {
            var result = await _cycleService.CreateCycleAsync(cycleDto);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            // Client error - return as 400
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log the exception with appropriate logging framework
            return StatusCode(500, new { message = $"An error occurred while processing your request {ex}" });
        }
    }

    // [HttpPut("{id}")]
    // public IActionResult UpdateCycle(int id, Cycle cycle)
    // {
    //     if (id != cycle.Id)
    //     {
    //         return BadRequest();
    //     }
    //     _cycleService.UpdateCycle(cycle);
    //     return NoContent();
    // }

    // [HttpDelete("{id}")]
    // public IActionResult DeleteCycle(int id)
    // {
    //     _cycleService.DeleteCycle(id);
    //     return NoContent();
    // }
}

