using System;
using System.ComponentModel.DataAnnotations.Schema;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public class BodyMetric : AuditFields
{
    // Core metrics
    public double WeightKg { get; set; }              // Required
    public double? HeightCm { get; set; }             // Optional but useful

    // Estimated values
    public double? BodyFatPercentage { get; set; }
    public double? MuscleMassKg { get; set; }

    // Derived metric
    public double? BMI { get; set; }

    public MeasurementMethod MeasurementMethod { get; set; }

    public DateTime RecordedAt { get; set; }

    // User relationship
    public string FemFitUserId { get; set; } = null!;
    [ForeignKey(nameof(FemFitUserId))]
    public FemFitUser FemFitUser { get; set; } = null!;
}

public enum MeasurementMethod
{
    BIA = 1,        // Smart scale
    DEXA = 2,       // Medical
    Skinfold = 3,
    ManualEntry = 4
}

