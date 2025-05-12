using System;

namespace FemFitPlus.Models.Dtos;

public class CycleUpdateDto
{
    public DateTime? EndDate { get; set; }
    public string? Phase { get; set; }
    public string? Symptoms { get; set; }
    public string? Mood { get; set; }
    public int EnergyLevel { get; set; }
}
