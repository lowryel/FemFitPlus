using System;
using System.ComponentModel.DataAnnotations.Schema;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public class Profile: AuditFields
{
    public string UserId { get; set; } = null!;
    public string? ProfileImage { get; set; }
    public double? HeightCm { get; set; }
    public double? WeightKg { get; set; }
    public string FitnessGoal { get; set; } = null!;
    public int CycleLength { get; set; }
    public int PeriodLength { get; set; }
    public string? LifestyleNotes { get; set; }
    public string? PreferredWorkoutType { get; set; }

    // Navigation property
    [ForeignKey("UserId")]
    public FemFitUser? Femfituser { get; set; }
}
