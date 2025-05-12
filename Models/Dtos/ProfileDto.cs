using System;

namespace FemFitPlus.Models.Dtos;

public class ProfileDto(string id, string userId, double? heightCm, double? weightKg, string fitnessGoal, int cycleLength, int periodLength, string? lifestyleNotes, string? preferredWorkoutType)
{
    public string Id { get; set; } = id;
    public string? UserId { get; set; } = userId;
    public double? HeightCm { get; set; } = heightCm;
    public double? WeightKg { get; set; } = weightKg;
    public string? FitnessGoal { get; set; } = fitnessGoal;
    public int? CycleLength { get; set; } = cycleLength;
    public int? PeriodLength { get; set; } = periodLength;
    public string? LifestyleNotes { get; set; } = lifestyleNotes;
    public string? PreferredWorkoutType { get; set; } = preferredWorkoutType;
}
