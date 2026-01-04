using System;
using System.Security.Claims;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services;
using FemFitPlus.Services.Filters;
using FemFitPlus.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FemFitPlus.Controllers;

[ApiController]
[Route("cycles")]
public class CycleController(ICycleService cycleService) : ControllerBase
{
    private readonly ICycleService _cycleService = cycleService;
    // Add methods for handling HTTP requests here
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCycleById(string id)
    {
        var cycle = await _cycleService.GetCycleByIdAsync(id, User.GetUserId());
        if (cycle == null)
        {
            return NotFound();
        }
        return Ok(cycle);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCyclesByUserId()
    {
        var userId = User.GetUserId();
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "User ID is required" });
            }

            var cycles = await _cycleService.GetCyclesByUserIdAsync(userId);

            if (cycles == null || cycles.Count == 0)
            {
                return NotFound(new { message = "No cycles found for this user" });
            }

            return Ok(cycles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred while retrieving cycles: {ex.Message}" });
        }
    }


    [Authorize]
    [HttpGet("filter")]
    public async Task<IActionResult> FilterCycles([FromQuery] CycleFilter filter)
    {
        try
        {
            var cycles = await _cycleService.Query(filter);
            if (cycles == null)
            {
                return NotFound(new { message = "No cycles found" });
            }
            return Ok(cycles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred while retrieving cycles {ex.Message}" });
        }
    }

    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllCycles()
    {
        try
        {
            var cycles = await _cycleService.GetAllCyclesAsync();
            return Ok(cycles);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = $"An internal server error occurred" });
        }
    }

    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateCycle([FromBody] CycleCreateDto cycleDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.GetUserId();
        Console.WriteLine($"User ID: {userId}");
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { message = "User not authenticated" });
        }
        cycleDto.UserId = userId;

        try
        {
            var result = await _cycleService.CreateCycleAsync(cycleDto);
            return StatusCode(201, new { cycle = result });
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

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCycle(string id, CycleUpdateDto cycle)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "Cycle ID is required" });
            }
            await _cycleService.UpdateCycleAsync(id, cycle);
            Response.Headers.Append("Access-Control-Allow-Origin", "*");
            return StatusCode(200, new { message = "Cycle updated successfully" }); ;
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An error occurred while updating the cycle: {ex.Message}" });
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCycle(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new { message = "Cycle ID is required" });
            }

            await _cycleService.DeleteCycleAsync(id, User.GetUserId());
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the cycle" });
        }
    }
}

