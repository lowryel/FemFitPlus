using System;
using System.ComponentModel.DataAnnotations.Schema;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public class BodyMetric: AuditFields
{
    public double WeightKg { get; set; }
    public double? HeightCm { get; set; } 
    public DateTime RecordedAt { get; set; }
    public string FemFitUserId { get; set; } = null!;
    [ForeignKey("FemFitUserId")]
    public FemFitUser? Femfituser { get; set; }
}

