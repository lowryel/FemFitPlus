using System;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public class WorkOut: AuditFields
{
    public string Title { get; set; } = null!;
    public string? ImageURL { get; set; }
    public string? Description { get; set; }
    public string? PhaseTargeted { get; set; }
    public string? Level { get; set; }
    public int Duration { get; set; }
    public string? VideoURL { get; set; }
    public string? Instructions { get; set; }
    public string? EquipmentNeeded { get; set; }
    public string? Notes { get; set; }
    public string? WorkoutType { get; set; }
    public WorkoutStatus WorkoutStatus { get; set; }
}


public class WorkoutHistory: AuditFields
{
    public string WorkoutId { get; set; }  = null!; // Foreign key to WorkOut
    public string? UserId { get; set; } // Foreign key to FemFitUser
    public DateTime CompletedAt { get; set; }

    public WorkOut? Workout { get; set; }
    public FemFitUser? Femfituser { get; set; }
}


public enum WorkoutStatus
{
    Assigned,
    Completed
}
