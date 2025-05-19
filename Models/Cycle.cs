using System;
using System.ComponentModel.DataAnnotations.Schema;
using FemFitPlus.Data;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public enum CyclePhase
{
    Menstrual,
    Follicular,
    Ovulation,
    Luteal
}

public class Cycle: AuditFields
{
    public string UserId { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public CyclePhase? Phase { get; set; }
    public string[]? Symptoms { get; set; }
    public string[]? Mood { get; set; }
    public int EnergyLevel { get; set; }
    [ForeignKey("UserId")]
    public FemFitUser? Femfituser { get; set; }
}
