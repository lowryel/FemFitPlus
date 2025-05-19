using System;

namespace FemFitPlus.Models.Dtos;

public class CycleDto
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string[]? Symptoms { get; set; } 
    public string[]? Mood { get; set; } 
    public int EnergyLevel { get; set; } // in days
    public CyclePhase Phase { get; set; } // in days
}
