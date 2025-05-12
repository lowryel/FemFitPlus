using System;

namespace FemFitPlus.Models.Dtos;

public class CycleCreateDto
{
    public string UserId { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public CyclePhase? Phase { get; set; }
    public string? Symptoms { get; set; }
    public string? Mood { get; set; }
    public int EnergyLevel { get; set; }
    // public FemFitUser? Femfituser { get; set; }
}
