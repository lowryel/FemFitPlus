using System;

namespace FemFitPlus.Models.Dtos;

public class ProfileCreateDto
{
    public string? UserId { get; set; } = null!;
    public double? HeightCm { get; set; }   
    public double? WeightKg { get; set; }
    public string? FitnessGoal { get; set; } = null!;
    public int CycleLength { get; set; }
    public int PeriodLength { get; set; }
    public string? LifestyleNotes { get; set; }
    public string? PreferredWorkoutType { get; set; }
}
