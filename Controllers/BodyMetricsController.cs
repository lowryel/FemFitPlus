using System;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services;
using Microsoft.AspNetCore.Mvc;

namespace FemFitPlus.Controllers;

[ApiController]
[Route("bodymetrics")]
public class BodyMetricsController(IBodyMetricService bodyMetricService) : ControllerBase
{
    private readonly IBodyMetricService _bodyMetricService = bodyMetricService;
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BodyMetricDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _bodyMetricService.Create(dto);

            return Ok(id);

        }
        catch (Exception)
        {

            throw new Exception("");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateBodyMetricDto dto)
    {
        if (id != dto.Id)
            return BadRequest("Mismatched metric ID.");

        var updated = await _bodyMetricService.Update(dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpGet("progress/weekly")]
    public async Task<IActionResult> GetWeeklyProgress(
    [FromQuery] string userId,
    [FromQuery] bool isPregnant = false)
    {
        var progress = await _bodyMetricService
            .GetWeeklyProgressAsync(userId, isPregnant);

        if (progress == null)
            return NoContent();

        return Ok(progress);
    }

    [HttpGet("progress/monthly")]
    public async Task<IActionResult> GetMonthlyProgress(
        [FromQuery] string userId,
        [FromQuery] bool isPregnant = false)
    {
        var progress = await _bodyMetricService
            .GetMonthlyProgressAsync(userId, isPregnant);

        if (progress == null)
            return NoContent();

        return Ok(progress);
    }
}
